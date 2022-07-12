using System.Diagnostics.Contracts;

namespace Infoware.ErrOr
{
    /// <summary>
    /// Wrap an exception to a type
    /// </summary>
    /// <typeparam name="TValue">Type wrapped</typeparam>
    public readonly struct ErrOr<TValue>
    {
        private readonly TValue? _value = default;
        private readonly Exception? _exception = null;

        [Pure]
        public static implicit operator ErrOr<TValue>(TValue value) => new(value);

        [Pure]
        public static implicit operator ErrOr<TValue>(Exception exception) => new(exception);

        /// <summary>
        /// Return inner value only if has a not null value
        /// </summary>
        public TValue Value => _value!;

        /// <summary>
        /// Return inner exception if it is an error
        /// </summary>
        public Exception Exception => _exception!;

        /// <summary>
        /// Create a new object that has resulted from a valid operation
        /// </summary>
        /// <param name="value">Value result of a valid operation</param>
        public ErrOr(TValue value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
            _exception = null;
        }

        /// <summary>
        /// Create a new object that has resulted from a failed operation
        /// </summary>
        /// <param name="e">Exception result of a failed operation</param>
        public ErrOr(Exception e)
        {
            _exception = e ?? throw new ArgumentNullException(nameof(e));
            _value = default;
        }

        /// <summary>
        /// Returns true if inner value is the result of a failed operation, otherwise false
        /// </summary>
        [Pure]
        public bool IsFaulted => _exception != null;

        /// <summary>
        /// Returns true if inner value is not null and is the result of a valid operation, otherwise false
        /// </summary>
        [Pure]
        public bool IsSuccess => _value != null;

        [Pure]
        public override string ToString() =>
            IsFaulted
                ? _exception?.ToString() ?? "(exception)"
                : _value?.ToString() ?? "(null)";

        /// <summary>
        /// Create branch code that executes functions based on the value set as the state of the object
        /// </summary>
        /// <typeparam name="TResult">Result of match operation</typeparam>
        /// <param name="funcSuccess">Executed if state object is success</param>
        /// <param name="funcFail">Executed if state object is failed</param>
        /// <returns></returns>
        [Pure]
        public TResult Match<TResult>(Func<TValue, TResult> funcSuccess, Func<Exception, TResult> funcFail) =>
            _exception != null
                ? funcFail(_exception)
                : funcSuccess(_value!);

        /// <summary>
        /// Convert typed ErrOr objet to another typed ErrOr object
        /// </summary>
        /// <typeparam name="TResult">Result of match operation</typeparam>
        /// <param name="funcMap">Mapping function of success state value</param>
        /// <returns></returns>
        [Pure]
        public ErrOr<TResult> ConvertTo<TResult>(Func<TValue, TResult> funcMap) =>
            _exception != null
                ? new(_exception)
                : new(funcMap(_value!));

        /// <summary>
        /// Convert typed ErrOr objet to another typed ErrOr object
        /// </summary>
        /// <typeparam name="TResult">Result of match operation</typeparam>
        /// <param name="funcMapAsync">Mapping function of success state value</param>
        /// <returns></returns>
        [Pure]
        public async Task<ErrOr<TResult>> ConvertToAsync<TResult>(Func<TValue, Task<TResult>> funcMapAsync) =>
            _exception != null
                ? new(_exception)
                : new(await funcMapAsync(_value!));
    }
}
