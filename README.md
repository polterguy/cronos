
# Cronos, algebraic date range operations

Imagine you have 2 employees; Alice and Bob, and you want to schedule a meeting between
yourself, Alice, and Bob. This creates a problem, which is to figure out what time both
you, Alice and Bob are available. To answer this question with Cronos is quite simple.
Simply create a union of the times you are busy, Alice is busy, and Bob is busy. Then
inverse this date range, and you're left with all possible date ranges when all 3 of
you are available for a meeting. Cronos supports the following date range operations.

* Union
* Intersections
* Inversion

The above 3 operations happens to be the equivalent of the following logical operations.

* OR
* AND
* NOT

Using these three algebraic operations, you can easily answer questions such as the following

* _"When is both Bob and Alice busy"_ - AND/Intersection of Alice and Bob's calendar activities
* _"When is neither of Alice and Boby at work"_ - Inverse of the union of Alice and Bob's calendar activities
* Etc ...

This makes calculus with date ranges fairly simple, and makes your code for doing such
calculations easily understood.

**Disclaimer** - Cronos is in alpha version at the moment, and not production ready. But if
you'd like to play around with it, you can clone its repository.
