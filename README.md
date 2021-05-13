# DiffChecker

REST API for calculating the differences between two base64 encoded strings. Written in C# with the ASP.NET core framework.

## Pre-Requisites

To run the api, it is necessary:

- .NET SDK
- MongoDB

It is necessary to have the mongo server running to run the integration tests and to run the api.

## Running the tests

From the command line, run dotnet test DiffChecker.sln

## Building the Project

From the command line, run:

```
dotnet publish DiffChecker.sln -o publish
```

and the contents will be built in the 'publish' folder.

## Running the API

Once the api is built, just run DiffChecker.Api.exe.

The ports used by the API will be 5000, for http requests, and 5001 for https requests.

## Documentation

Once the application is running, the Swagger API documentation will be served at the root of the server.

It will include information on all endpoints and possible return values.

## Using the API

For setting the data on the endpoints

```
http://localhost:5000/v1/diff/{id}/left
http://localhost:5000/v1/diff/{id}/right
```

send a POST request with the header

```
Content-Type: application/json
```

and the body

```
{
	"data" : "VGhpcyBpcyBhIHRlc3Q="
}
```

The response will have the id and the data sent:

```
{
    "requestId": "1",
    "data": "VGhpcyBpcyBhIHRlc3Q="
}
```

To perform the comparison, send a GET request with the id with the set data.

```
http://localhost:5000/v1/diff/{id}/left
```

## Examples of Result

Given two identical strings, the output of the comparison will be:

```
{
    "equal": true
}
```

Given strings with different lengths, the output will be:

```
{
    "differentSize": true
}
```

Given different strings with the same lengths, the output will be in the following format:

```
{
    "equal": false,
    "diffPoints": [
        {
            "offset": 5,
            "length": 2
        }
    ]
}
```

## Error Response

If any request ends up in error, for example, failure to decode a given string from base64, the API will return an output in the format:

```
{
    "StatusCode": 500,
    "Message": "Error converting VGhpcyBiZSBhIHRlc3Q from base64."
}
```

## Improvement suggestions

Some of the possible improvements to this project include:

- Dockerizing the API
- Returning the actual diffs in the input data
- Increasing test code coverage
- Adding new appsettings for different environments

## About

Developed by Juliano Ladeira

julianoladeira [at] gmail.com




