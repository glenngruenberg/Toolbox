using System;

namespace Option
{
    public abstract class Option<T>: IEquatable<Option<T>>
    {
        public static implicit operator Option<T>(T value) =>
            value == null
            ? (Option<T>)None.Value
            : new Some<T>(value);

        public static implicit operator Option<T>(None none) => new None<T>();

        public abstract Option<TResult> Apply<TResult>(Func<T, TResult> function);

        public abstract Option<TResult> Apply<TResult>(Func<T, Option<TResult>> function);

        public abstract T ResultOr(T otherwise);

        public abstract T ResultOr(Func<T> otherwise);

        public Option<TNew> OfType<TNew>() where TNew : class =>
            this is Some<T> some && typeof(TNew).IsAssignableFrom(typeof(T))
                ? (Option<TNew>) (some.Content as TNew)
                : None.Value;

        public bool Equals(Option<T> other) => Equals((object) other);

        public override bool Equals(object obj) =>
            this is Some<T> ? ((Some<T>) this).Equals(obj) : ((None<T>) this).Equals(obj);

        public override int GetHashCode() => 
            this is Some<T> ? ((Some<T>) this).GetHashCode() : ((None<T>) this).GetHashCode();

        public static bool operator ==(Option<T> a, Option<T> b) => a is null && b is null || !(a is null) && a.Equals(b);

        public static bool operator !=(Option<T> a, Option<T> b) => !(a == b);
    }

    public sealed class Some<T> : Option<T>
    {
        public T Content { get; }

        internal Some(T value)
        {
            Content = value;
        }

        public static implicit operator T(Some<T> some) => some.Content;

        public override Option<TResult> Apply<TResult>(Func<T, TResult> function) => function(Content);

        public override Option<TResult> Apply<TResult>(Func<T, Option<TResult>> function) => function(Content);

        public override T ResultOr(T otherwise) => Content;

        public override T ResultOr(Func<T> otherwise) => Content;

        public override int GetHashCode() => Content.GetHashCode();

        public override bool Equals(object obj) => !(obj is null) && obj is Some<T> some && Equals(some);

        private bool Equals(Some<T> other) => !(other is null) && Content.Equals(other.Content);
    }

    public sealed class None<T> : Option<T>
    {
        internal None() { }

        public override Option<TResult> Apply<TResult>(Func<T, TResult> function) => None.Value;

        public override Option<TResult> Apply<TResult>(Func<T, Option<TResult>> function) => None.Value;

        public override T ResultOr(T otherwise) => otherwise;

        public override T ResultOr(Func<T> otherwise) => otherwise();

        public override int GetHashCode() => None.NoneHash;

        public override bool Equals(object obj) => obj is None<T> || obj is None;
    }

    public sealed class None
    {
        internal const int NoneHash = 0;

        public static None Value { get; } = new None();

        private None() { }

        public override bool Equals(object obj) => obj is None || IsGenericNone(obj.GetType());

        private static bool IsGenericNone(Type type) =>
            type.GenericTypeArguments.Length == 1 &&
            typeof(None<>).MakeGenericType(type.GenericTypeArguments[0]) == type;

        public override int GetHashCode() => NoneHash;
    }
}
