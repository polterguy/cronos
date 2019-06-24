
# Cronos, algebraic date range operations

Imagine you have 2 employees; Alice and Bob, and you want to schedule a meeting between
yourself, Alice, and Bob. This creates a problem, which is to figure out what time both
you, Alice and Bob are available. To answer this question with Cronos is quite simple.
Simply create a union of the times you are busy, Alice is busy, and Bob is busy. Then
inverse this date range, and you're left with all possible date ranges when all 3 of
you are available for a meeting. Cronos supports the following date range operations.

* Union - OR
* Intersections - AND
* Inversion - NOT

Using these three algebraic operations, you can easily answer questions such as the following

* _"When is both Bob and Alice busy?"_ - AND/INTERSECTION of Alice's and Bob's calendar activities
* _"When is either Alice or Bob at work?"_ - OR/UNION of Alice's and Bob's calendar activities
* _"When is neither of Alice and Boby at work?"_ - NOT/INVERSE of the UNION of Alice's and Bob's calendar activities
* Etc ...

This makes calculus with dates and calendars fairly simple, and makes your code for doing such
calculations easily understood. Below is an example of working code, assuming you implement
the missing `GetCalendar` method.

```csharp
/*
 * Retrieves Alice's and Bob's calendar (somehow).
 * Implement GetCalendar yourself, any ways you see fit.
 */
DateRangeCollection alice = GetCalendar("Alice");
DateRangeCollection bob = GetCalendar("Bob");

/*
 * Calculates availability for both Alice and Bob, based
 * upon their existing calendar activities.
 *
 * This first creates a UNION of Alice and Bob's
 * calendar activities, for then to INVERSE that result.
 * The results is a DateRangeCollection of when both Alice
 * and Bob are available for a meeting.
 */
DateRangeCollection availability = !(alice | bob);

/*
 * Calculates when both Alice and Bob are busy.
 */
DateRangeCollection bothBusy = alice & bob;

/*
 * Calculates how many hours both Alice and Bob are busy.
 */
TimeSpan bothBusyHours = bothBusy.Size;

/*
 * The following finds the first opening in the above dataset
 * that is larger or equal to 2 hours.
 *
 * This code assumes you include System.Linq in your C# code file.
 */
DateRange availableForMeeting = availability.FirstOrDefault(x => x.Size >= new TimeSpan(2,0,0));

```

Cronos contains two main classes.

* __DateRange__ - A start DateTime and an end DateTime, plus helper methods
* __DateRangeCollections__ - A list of DateRange instances

Using these two methods you can perform algebraic operations on calendar items to answer
any questions such as the above. Both classes are immutable, and hence also thread safe,
since an instance can never be modified. So each instance can be safely shared among
multiple threads.

**Notice** - When you create an instance of a `DateRangeCollection`, its items are
_"normalized"_, which implies creating a UNION out of its values. This implies that
even though you add 5 items for instance, if two of these items are overlapping each
other, the resulting DateRangeCollection will only contain 4 items.

## DateRange methods and operators

* Start - Start DateTime
* End - End DateTime
* Size - Size of instance as a TimeSpan
* Intersects - Returns true if DateRange intersects with the specified parameter
* Adjacent - Returns true if DateRange is adjacent to the specified parameter
* Union - Creates a UNION of the given instance combined with the specified parameter
* Intersection - Returns the intersection of the given instance combined with the specified parameter

In addition DateRange overloads all relevant operators, such as ==, !=, >, <, >= and <=.

**Disclaimer** - Cronos is in alpha version at the moment, and not production ready. But if
you'd like to play around with it, you can clone its repository.
