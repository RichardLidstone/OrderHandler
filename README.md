# OrderHandler

A test project simulating an ever increasing number of orders being generated and consumed by a user determined number of processors.

You said to have fun, and I did - it was quite an interesting exercise. 
Looking forward to going through it with you and justifying the bad bits (there are bad bits).

## Known Issues

- Design of the system needs a bit of refactoring - time constraint got to me.
- CTRL+C unceremoniously kills everything and exits with a horrendous exit code - there's no attempt to gracefully shut down and clean up.
- Unit testing is incomplete to put it kindly.
- Warnings due to using async without an await in the test project, but there would likely be IO in a real system.
- Warnings due to using async to start an async Task deliberately without awaiting - we should use a cancellation token to clean these up gracefully.
