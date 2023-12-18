# Stock Quote Alert

## Summary

Stock Quote Alert is a C# CLI (Command Line Interface) application to continuously monitoring the stock price of a B3 asset while it is running, with the objective of notifying, via email, if the price of this asset falls more than a certain level, or rises above another.

## Description

In this project, the free trial of [Bravi API](https://brapi.dev) was used to monitoring the stock price, with data been updated every 30 minutes.
Furthermore, for the SMTP server, the [MailTrap](https://mailtrap.io/) free trial was used to test the sending and receiving of emails.

## Setup

Before running the project, you have to create a settings.json in the same directory as the compiled file StockQuoteAlert.exe. The .json file has to look like this:

```json
{
  "Api": {
    "Token": "API_TOKEN",
    "Delay": 1800000 // Delay of the API in milliseconds
  },
  "Smtp": {
    "Server": "SERVER_NAME",
    "Port": 587, // or 25 or 465 or 2525
    "Username": "USER_NAME",
    "Password": "PASSWORD"
  },
  "Sender": "sender@example.com",
  "Recipients": [
    "recipient1@example.com",
    "recipient2@example.com",
    "recipient3@example.com" // You can put how many recipient emails you want
  ]
}
```

## Compiling and Running

For running this project in your machine, you must follow these steps:

- Clone this repository
- Compile the application
- Create the settings.json file
- Open the terminal on the directory or run in you IDE with the following arguments:
  1. The asset to be monitored
  2. The reference price for sale
  3. The reference purchase price
- While running, the application will send e-mails to the recipients when the asset price reach the upper bound or the lower bound, recommending the stock sale or purchase, respectively
