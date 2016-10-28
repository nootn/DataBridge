___________________________
Why are the tests numbered?
---------------------------

Because they are integration tests, not unit tests the order is significant.
Notice I did not say the order "matters", it's just "significant".

The tests can be run in any order, that is fine.  However, due to the procedural
nature of the program, if an early test fails then it is possible later
tests will also fail because there is some issue in program logic before
the code the later test is really checking (so the code targeted in the
later test is actually fine, some preceding logic is wrong which should be 
tested in an "earlier numbered" test).

Each test does not require any other test to run before it - they can be run
independently.  This may come back to bite me because if I change some early
program logic, I may have to go and change a lot of tests.  I think that
is just the nature of integration tests though, unless I am missing something?
Hopefully code re-use in the tests saves me a bit here.

So the ordering just gives you an order to fix issues in if there are broken tests.
Fix the lower numbered tests first, it may just happen to fix the later numbered tests!

_____________________________________________
Why the weird numbering convention of _####_?
---------------------------------------------

I wanted to name the class with the number at the start but C# does not allow that,
hence the preceding underscore.  Then with the numbering, I am guessing I won't have
anywhere near 1000 test scenarios in any folder, hence the 4 digits.  I am making the
tests increment in values of 10 by default, that way between two tests that I think
are sequential at the time of writing them, I can think of 9 more to slip in between
without having to re-order all the tests! (I.e. if I named them sequentially I would
forever be renaming ones down the list every time I add one in).  
Is it perfect? NO!
Is it good?    Probably not!
Oh well, such is life.
If you have a better way to do this please let me know: http://nootn.com.au