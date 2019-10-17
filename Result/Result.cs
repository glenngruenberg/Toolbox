using System;

namespace Result
{
    public class Result<TSuccess, TError>
    {
        private readonly TSuccess _success;
        private readonly TError _error;
        private readonly bool _isSuccess;

        public Result(TSuccess success)
        {
            _success = success;
            _isSuccess = true;
        }

        public Result(TError error)
        {
            _error = error;
            _isSuccess = false;
        }

        public static implicit operator Result<TSuccess, TError>(TSuccess success) => 
            new Result<TSuccess, TError>(success);

        public static implicit operator Result<TSuccess, TError>(TError error) => 
            new Result<TSuccess, TError>(error);

        public TOut Merge<TOut>(Func<TSuccess, TOut> onSuccess, Func<TError, TOut> onError) =>
            _isSuccess
            ? onSuccess(_success)
            : onError(_error);

        public Result<TOut, TError> OnSuccess<TOut>(Func<TSuccess, Result<TOut, TError>> onSuccess) =>
            _isSuccess
            ? onSuccess(_success)
            : _error;

        public Result<TSuccess, TOut> OnError<TOut>(Func<TError, Result<TSuccess, TOut>> onError) =>
            _isSuccess
            ? _success
            : onError(_error);
    }
}
