using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static GloEpidBot.Utilities.Responses;

namespace GloEpidBot.Utilities
{
    public class Responses
    {
        public interface IResp
        {
            string Message { get; set; }
            bool DidError { get; set; }



        }


        public interface IResponse
        {
            string Message { get; set; }

            bool DidError { get; set; }

            string ErrorMessage { get; set; }
        }

        public interface ISingleResponse<TModel> : IResp
        {
            Data<TModel> data { get; set; }
            Link link { get; set; }


        }

        public interface IListResponse<TModel> : IResp
        {
            DataList<TModel> data { get; set; }


        }

        public interface IPagedResponse<TModel> : IListResponse<TModel>
        {
            int ItemsCount { get; set; }
            PaginationLink link { get; set; }
            double PageCount { get; }
        }

        public class Response : IResp
        {
            public string Message { get; set; }

            public bool DidError { get; set; }

            public string ErrorMessage { get; set; }

        }










        public class ErrorResponse
        {
            public string Status { get; set; }
            public string Title { get; set; }
            public string Details { get; set; }
        }
        public class ErrorBoss
        {
            public string Message { get; set; }

            public bool DidError { get; set; }
            public List<ErrorResponse> Errors { get; set; }
        }


        public class SingleResponse<TModel> : ISingleResponse<TModel>
        {

            public string Message { get; set; }

            public bool DidError { get; set; }


            public Data<TModel> data { get; set; }


            public Link link { get; set; }

        }

        public class ListResponse<TModel> : IListResponse<TModel>
        {
            public string Message { get; set; }

            public bool DidError { get; set; }

            public string ErrorMessage { get; set; }
            public int ItemCount { get; set; }


            public DataList<TModel> data { get; set; }

        }

        public class PagedResponse<TModel> : IPagedResponse<TModel>
        {
            public string Message { get; set; }

            public bool DidError { get; set; }

            public string ErrorMessage { get; set; }

            public DataList<TModel> data { get; set; }

            public int PageSize { get; set; }

            public int PageNumber { get; set; }

            public int ItemsCount { get; set; }

            public PaginationLink link { get; set; }

            public double PageCount
                    => ItemsCount < PageSize ? 1 : (int)(((double)ItemsCount / PageSize) + 1);
        }


    }
    public class MessageResponse
    {
        public string Status { get; set; }
    }
    public static class ResponseExtensions
    {
        public static IActionResult ToHttpResponse(this IResponse response)
        {
            var status = response.DidError ? HttpStatusCode.InternalServerError : HttpStatusCode.OK;

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
        }

        public static IActionResult ToHttpResponse<TModel>(this ISingleResponse<TModel> response)
        {
            var status = HttpStatusCode.OK;

            if (response.DidError)
                status = HttpStatusCode.InternalServerError;
            else if (response.data == null)
                status = HttpStatusCode.NotFound;

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
        }

        public static IActionResult ToHttpResponse<TModel>(this IListResponse<TModel> response)
        {
            var status = HttpStatusCode.OK;

            if (response.DidError)
                status = HttpStatusCode.InternalServerError;
            else if (response.data == null)
                status = HttpStatusCode.NoContent;

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
        }

    }
    public class Error
    {
        string code { get; set; }
        string source { get; set; }
        string title { get; set; }
        string details { get; set; }
    }
    public class Data<TModel>
    {
        public string type { get; set; }
        public string Id { get; set; }
        public TModel attributes { get; set; }
        public Relationship relationship { get; set; }


    }

    public class DataList<TModel>
    {
        public string type { get; set; }
        public string Id { get; set; }
        public IEnumerable<TModel> attributes { get; set; }
        public Relationship relationship { get; set; }


    }


    public class Relationship
    {
        public Link link { get; set; }
        public RelationshipData data { get; set; }


    }
    public class RelationshipData
    {


        public string type { get; set; }
        public string Id { get; set; }
        public object data { get; set; }
    }
    public class Link
    {
        public string description { get; set; }

        public string self { get; set; }
        public string related { get; set; }
    }
    public class PaginationLink
    {
        public string self { get; set; }
        public string first { get; set; }
        public string last { get; set; }
        public string prev { get; set; }
        public string next { get; set; }
        public string related { get; set; }

    }

}
