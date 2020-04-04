# Nysa
This repository contains a set of simple functional programming libraries in C#.

## Nysa.Logics
This is the primary functional programming library. It contains the following elevated types:
1. `Option<T>` for values of `T` that could be `Some<T>` or `None<T>`.
2. `Suspect<T>` for values of `T` that could be `Confirmed<T>` or `Failed<T>`.
3. `Try<T>` for values of `Suspect<T>` that maybe `Pending<T>` (i.e., not yet taken from it's function that may throw an exception) or `Resolved<T>`.

For these elevated types, Nysa.Logics defines functions (using extension methods) in the following general catagories (given an elevated type `E<>` and the outer Func is the extension method definition):
* Return - `Func<T, E<T>>`
* Map - `Func<E<T>, Func<T, R>, E<R>>`
* Bind - `Func<E<T>, Func<T, E<R>>, E<R>>`
* Match - `Func<E<T>, Func<T, R>, R, R>`
* Match - `Func<E<T>, Func<T, R>, Func<R>, R>`
* Apply - `Func<E<Func<T, R>>, E<T>, Func<E<R>>>`
* Apply - `Func<E<Func<T1, T2, TR>>, E<T1>, E<Func<T2, TR>>>`
