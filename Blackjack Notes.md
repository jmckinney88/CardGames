# Blackjack Notes

## Keep your code dry
Dry means Dont Repeat Yourself. If you say the same thing twice, it can probably be written once and reused.

## Single Responsibility
Every class, function, etc. should have a single responsibility. For me, I think of this as I should be able to succinctly explain the role of any object.

## Keep it simple
Keep code simple and self documenting. If you find yourself going on for a while in a method with various branches, consider moving some of the logic into helpers with descriptive names. Even if they dont get reused, it still makes it easier to understand what a segment of code is doing when you come back to the code tomorrow or 6 months from now.

## Strategy Pattern
Avoid hardcoding the implementation of a classes dependencies, **especially** when it comes to static references.