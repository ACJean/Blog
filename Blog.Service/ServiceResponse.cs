using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service
{
    public class ServiceResponse<ResponseType>
    {

        public ServiceResponse(bool success, string message, ResponseType data)
        {
            IsCompleted = true;
            IsSuccess = success;
            Message = message;
            Data = data;
            Exception = null;
            ValidationMessages = null;
        }

        public ServiceResponse(string[] validationMessages)
        {
            if (validationMessages == null || (validationMessages != null && validationMessages.Length == 0)) throw new Exception("La lista de mensajes de validación no contiene datos");
            IsCompleted = true;
            IsSuccess = false;
            Message = null;
            Data = default;
            Exception = null;
            ValidationMessages = validationMessages;
        }

        public ServiceResponse(Exception exception)
        {
            if (exception == null) throw new Exception("La excepción no contiene datos");
            IsCompleted = false;
            IsSuccess = false;
            Message = null;
            Data = default;
            Exception = exception;
            ValidationMessages = null;
        }

        public bool IsCompleted { get; }
        public bool IsSuccess { get; }
        public string Message { get; }
        public ResponseType Data { get; }
        public Exception Exception { get; }
        public string[] ValidationMessages { get; }

        public static ServiceResponse<ResponseType> Completed(bool success, string message = null, ResponseType data = default)
        {
            return new ServiceResponse<ResponseType>(success, message, data);
        }

        public static ServiceResponse<ResponseType> Invalid(string[] validationMessages)
        {
            return new ServiceResponse<ResponseType>(validationMessages);
        }

        public static ServiceResponse<ResponseType> Error(Exception exception)
        {
            return new ServiceResponse<ResponseType>(exception);
        }

        public static int GetStatusCreated(ServiceResponse<ResponseType> serviceResponse)
        {
            if (serviceResponse.IsSuccess) return 201;
            else return GetStatus(serviceResponse);
        }

        public static int GetStatus(ServiceResponse<ResponseType> serviceResponse)
        {
            if (serviceResponse.IsSuccess) return 200;
            else if (serviceResponse.IsCompleted) return 400;
            else return GetStatusException(serviceResponse.Exception);
        }

        private static int GetStatusException(Exception ex)
        {
            return 500;
        }

    }
}
