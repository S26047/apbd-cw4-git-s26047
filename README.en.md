# refactoring-renewal-example

In this task, your goal is to refactor the code of the application named `LegacyRenewalApp`.

We assume that the application already works correctly from the business perspective and you are not supposed to change its functionality. Your task is only to improve the quality of the code.

The `LegacyRenewalAppConsumer` project acts as client code. You must not modify this project. Everything inside `LegacyRenewalApp` may be changed as long as you do not break the public contract used by `LegacyRenewalAppConsumer`.

Additionally, you must not modify the `LegacyBillingGateway` class. We assume that this is an external static class coming from an old library. If you want to reduce coupling to it, you need to wrap it in your own abstraction.

## Goal of the task

This task is meant to verify understanding of:

- DRY,
- SOLID principles,
- cohesion,
- coupling,
- safe refactoring of working code.

## What is problematic in the current code

The `SubscriptionRenewalService` class contains one long method that mixes many responsibilities:

- input validation,
- loading data from repositories,
- discount business logic,
- extra fee logic,
- tax logic,
- invoice object creation,
- invoice persistence,
- email sending.

In addition, the code contains:

- large `if-else` blocks,
- direct creation of dependencies inside the service,
- direct usage of an external static class,
- logic that could later be moved into dedicated business classes or services.

## What is expected from the refactoring

The student should:

- split the code into clear responsibilities,
- improve class cohesion,
- reduce coupling,
- remove duplication,
- extract parts of the logic into dedicated classes,
- consider replacing selected `if-else` blocks with interfaces and implementations according to the Open/Closed Principle,
- reduce direct dependencies by introducing abstractions and simple IoC through dependency injection,
- wrap `LegacyBillingGateway` in a custom class or interface without modifying the static class itself.

## Important constraints

- Do not change the code in `LegacyRenewalAppConsumer`.
- Do not change the `LegacyBillingGateway` class.
- Preserve the public contract used by the client code.
- Assume that the current logic is correct. The refactoring should improve code quality, not change the observable behavior from the client perspective.
