﻿// -----------------------------------------------------------------------
//  <copyright file="UnitOfWorkAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-09 22:07</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using OSharp.AspNetCore.UI;
using OSharp.Entity;


namespace OSharp.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// 自动事务提交过滤器，在<see cref="OnResultExecuted"/>方法中执行<see cref="IUnitOfWork.Commit()"/>进行事务提交
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UnitOfWorkAttribute : ActionFilterAttribute
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// 初始化一个<see cref="UnitOfWorkAttribute"/>类型的新实例
        /// </summary>
        public UnitOfWorkAttribute()
        {
            _unitOfWork = ServiceLocator.Instance.GetService<IUnitOfWork>();
            System.Diagnostics.Debug.WriteLine($"_unitOfWork: {_unitOfWork.GetHashCode()}");
        }

        /// <inheritdoc />
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.Result is JsonResult result)
            {
                if (result.Value is AjaxResult ajax && ajax.Type == "Error")
                {
                    return;
                }
            }
            _unitOfWork?.Commit();
        }
    }
}