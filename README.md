# Xero “RefactorThis” Project

## Initial steps

- Installed the application on my Windows machine.
- Updated the project from `v4.5.2` → `v4.8`. Updating the major version would likely break the application however updating the minor version would be beneficial for security.

## Separating and renaming the models

Separated all the models into their own class files.

- Product
- Products
- ProductOption
- ProductOptions

## Renamed the project to ProductService

There were issues with inconsistent namespacing, where there would be instances of `refactor_this` crossing over with `refactor_me`. This was especially noticeable with creating new classes where they would reference the wrong namespace. Snake casing is also an issue when everything else in the project uses UpperCamelCase.

## Moved the connection string out of the helpers file and into the web config file & Deleted the Helpers file

Having a helpers file in the Models directory felt very inappropriate. That’s core functionality that needs to be initialised on startup. The connection string also works in the web config file as well, and developers can target specifically which environment they want the database to be connected.

- Unsure about which configuration I would target for the `Database.mdf` file, however I’d assume it would go into debug rather than production.

## Implemented the DbContext for the Product Service

This is step one of the critical part of this refactor. Instead of relying on an SQL query fest in the controller, I made the switch to use the Entity Framework extensively, so that the application can benefit from features such as DTOs, migrations, seeding, etc. DbSets for Products and ProductOptions were also created.

## Added DTOs for both Product and Product Option

Allows for an abstraction of the data we want to represent to the client, and further allows for extendibility to prevent errors for anything that is added to the models

## Removed methods from models

Methods do not belong in models, business logic is better suited for controllers

## Removed usused isNew from models

Since the methods have been removed from models, the isNew attribute is no longer needed.

## Added foreign key “ProductId” to Product Option model

Now that we are interacting with our database at a greater level, it’s better that we utilise relational tables so that we can leverage SQL relations rather than manual relations in code.

## Added required keyword to Name

Prevent unnamed products or product options from being created

- There would have been more opportunities to create further table constraints across the board, given more time I would have considered it further

## Added migrations and seeds

No more breaking databases and not knowing how to deal with them. These allow a database to be safely scaffolded without harming the contained data.

[Use Code First Migrations to Seed the Database](https://docs.microsoft.com/en-us/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-3)

If the Database.mdf breaks for some reason, no problem! Just re-migrate a new database and seed it with the initial data provided.

## Overhauling the products controller

### Added better return types

Utilising return types such as `IQueryable<ProductDTO>` and `Task<IActionResult>` made better sense working with Entity Framework moving forward. This way the browser or application receiving responses from this service understands what it’s receiving and why.

### Added response types to GetOne, Create, Update, and Delete endpoints

Better for documenting the information that is being sent

### Switching out to Entity Framework style querying

It looks nicer and it’s easier to read than raw SQL queries. It works with Intellisense and has a decent amount of documentation out of the box.

[Create Data Transfer Objects (DTOs)](https://docs.microsoft.com/en-us/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-5)

### Search uses a simple string contains method

Felt keeping this one simple was necessary `...Where(p => p.Name.ToLower().Contains(name.ToLower()))...`

## Final Notes

While this is an improvement to the original design, there are definitely areas where I could have improved the application further. There was something that’s making the app run very slow and that would have been something I’d like to tackle.

I wanted to separate the Products Controller into `Products` and `ProductOptions`, however the API endpoints were incredibly intertwined having product options depend on products, I felt there was an argument against refactoring that. If clients are using this API, it makes it hard to overwrite that aspect of the application so I wanted to preserve that just as much as I wanted to refactor it entirely. Thus the controller still remains to be large in size.

I would also have liked to add something to the home content rather than hitting a forbidden 403 page.

Lastly, I tried running this on my Macbook to find that there were too many caveats to getting it to work on MacOS. Visual Studio is really not going well over there! It would have been nice to use something like VSCode on my Mac, but having an application build back using 2014 was never going to play well... at least now we have .NET Core :)
