# Pre-requisities
- Git
- Code editor (e.g Visual Studio / VSCode)
- SQL Server 2014+
- .NET 5

# Exercise Description
The following code test was created to evaluate your abilities as a .NET Developer. Some of the areas that you'll be evaluated in are:

- Creativity
- Proactivity
- Code quality
- Code organization
- Tooling choices
- Ability to follow instructions
- Attention to details

We would like you to solve each task as they appear below, and avoid doing early optimizations or refactoring unless specified or if it blocks your solution for a specific problem. If any code is copied from the internet, make sure to add a comment with a reference to where you sourced it from.

Please treat this project and the tasks below as if they were part of, or the beginning of a bigger application that you would build and support. That means, please write the code as *you* like to write it, not the way you think *we* would like you to write it.

# Getting started
1. Initialise a new git repository at the root of the folder and perform an initial commit.
2. Restore the `Backend-TakeHomeExercise.bak` backup onto a SQL Server installation. There is a single table called `Listings` that you'll work with
3. Open the `TakeHomeExcercise.sln` solution and confirm you can build and run the API. You should see "TODO" being returned from the `/listings` route.
4. Happy coding. Good luck, and have fun! :)

# How to do the exercise
As you work on each task please prefix your commit(s) message(s) with the task reference using the following pattern:

> [Task-XX] My commit message

When you complete a task, as part of your commit and changes, please change the corresponding task status below from `Pending` to `Completed` and add any comments into the designated area at the bottom of each task, where you can explain what problems you found and how you solved them. You can also explain how you'd do something more complex and/or time consuming if you had the time for it, feel free to also add any other comments that are relevant to the task.

If you have made any changes to the DB (changed schema, added indexes, foreign keys, etc), please include what you've done in the comments section of the relevant section.

An example of the expected information in a comments section could be:

```
- Code currently does XXX. A possible enhancement later could be to do ZZZ
- Wasn't sure what was intended for scenario YYY here, so i made the call to do ZZZ
```


# Tasks

---

## `Task-01`: Create an API endpoint to return paged listings

_Status: `Completed`_

Create an API endpoint that returns paged listings from the sample DB, given the following (optional) filters:

- Suburb (e.g "Southbank")
- CategoryType (e.g "Rental")
- StatusType (e.g "Current")
- Skip (e.g 0)
- Take (e.g 10)

Fields should be validated for invalid input, and the API should return an appropriate HTTP response when validation fails.

An example URL might be: `/listings?suburb=Southbank&categoryType=Rental&statusType=Current&take=10`

The returned JSON should look like:

```
{
  "items": [
    {
      "listingId": 4,
      "address": "53-55 Ellison St, Clifton Beach QLD 4879", // combination of address fields
      "categoryType": "Residential", // 1 = Residential, 2 = Rental, 3 = Land, 4 = Rural
      "statusType": "Current", // 1 = Current, 2 = Withdrawn, 3 = Sold, 4 = Leased, 5 = Off Market, 6 = Deleted
      "displayPrice": "Mid to High $800,000's",
      "title": "Buy me now!"
    }
  ],
  "total": 1212
}
```

```
Add comments here
- One of the assumptions I made when starting this exercise was the use of '=' when getting data for Suburb and not to use 'like' clause

- I am treating the listings table as the MaterializedView. Possible enhancements include adding fields like 
- StateCode, StateId, CategoryTypeCode, CategoryTypeName, StatusTypeCode, StatusTypeName.

- The transaction tables need to be seperate tables and the listings table would get updated by adding a message in the ServiceBus to achieve Eventual Consistency.
- For now, I have just created Enumerations for CategoryType and StatusType. 

- I wasn't sure of the validation messages required for the optional parameters so I added the data annotations to the RequestObject which throw error if incorrect values are being passed.

- 
```

---

## `Task-02`: Add caching by suburb

_Status: `Completed`_

A common use case for listings is returning current listings of a given type in a suburb. Because of this, we would like some basic caching adding to avoid the trip to the DB.

Please add this caching functionality, so that the following behavior occurs:
1. App loaded
2. `GET: /listings?suburb=Southbank&categoryType=Residential&take=10` -> cache MISS
2. `GET: /listings?suburb=Southbank&categoryType=Residential&take=10` -> cache HIT
3. `GET: /listings?suburb=Southbank&categoryType=Rental&take=10` -> cache MISS
4. `GET: /listings?suburb=Southbank&categoryType=Rental&take=10` -> cache HIT
5. `GET: /listings?suburb=Southbank&categoryType=Rental&take=5` -> cache HIT

```
Add comments here
- I have added the Response Cache which caches the results for 30 seconds. 
- Added the InMemory Cache Manager (Default to 120 seconds) on business layer to get data from cache instead of getting from DB every time
- On a production server where we can have more than 10k TPS, I would go with Redis Cache
- Generally, the caching is not added for the dynamic data and is only required for the data which is independent of user changes. 
- The data being cached here is too dynamic - based on user parameters and values. 
- Caching is expensive and should not be too dynamic like in this case. 
- We can use background jobs to cache data (if Caching for such data is required)
- Try to avoid external inputs as cache keys. Always set your keys in code. I have set the external inputs as cache keys which is not ideal
- Added 1 column for development testing to check if the data is from Cache or DB
```
---

## `Task-03`: Add a new property shortPrice

_Status: `Completed`_

We would like a new prop added to the payload:
```
{
   ... // existing props
   "shortPrice": "$800k"
}
```

This is a short version of the `displayPrice`, to be used on things like the pin display.

Minimum scenarios to handle:

| displayPrice           | shortPrice |
| ---------------------- | ---------- |
| $100                   | "$100"     |
| $100,000               | "$100k"    |
| $1,500,000             | "$1.5m"    |
| For Sale               | ""         |

Bonus points for handling other scenarios (use your judgement as to what the value should be)

```
Add comments here
- Added a UDF in SQL server - ufnGetShortPrice to get the Short Price. Added in GIT as well
- Does not cover all the scenarios and has some flaws in getting the Short price but updates a lot of rows
- Added 3 indexes - NonClusteredIndexes.sql (Added in GIT) to speed up the data retrieval. 
- We can discuss later why I chose these 3 indexes
```

---

## `Task-04`: Tests

_Status: `Completed`_

It is expected that you write tests for important changes you made above on the previous tasks, if any test was relevant. 
As a last pass, please check any code, existing or new that would benefit from tests and write them. 
Explain below the benefits of the tests you wrote and why they are important.

```
Add comments here
- A possible enhancement is to mock the actual data. 
- Since the focus is more on completing the Test Exercise and logic, I am skipping the mocking of data for now.
- If user changes data then ofcourse my test data might break. Thats why we need to mock the data.

- Test 1 - Check what happens when no values are passed. Does it use default values and fetch the data?
- Test 2 - Check what happens, if all the parameters (Suburb, CategoryType, StatusType) are being passed with the correct values. Does it fetch the data?
- Test 3 - Check what happens if the parameters with incorrect values such as Suburb ="South" are passed.
- Test 4 - Check if data is coming from cache when called within next 2 minutes.
- Test 5 - Check what happens for Skip and Take Data Annotations

- Other Unit Tests that I could write (considering the default values for Skip and Take)
- Test 6 - Should find values if only valid Suburb and CategoryType are being passed
- Test 7 - Should find values if only valid Suburb and StatusType are being passed
- Test 8 - Should find values if only valid CategoryType and StatusType are being passed
- Test 9 - Should find values if only valid Suburb is being passed
- Test 10 - Should find values if only valid CategoryType is being passed
- Test 11 - Should find values if only valid StatusType is being passed
- Test 12 - Check if cache expires after 2 minutes or not.

```

---
