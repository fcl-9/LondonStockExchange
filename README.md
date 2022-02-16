# LondonStockExchange
This project contains five different projects.
- LondonStockExchange.DataProducer - Opens an Swagger page which can be used to ingest data into the system.
- LondonStockExchange.DataProcessing.Tests - This project contains test for all projects in the system.
- LondonStockExchange.DataProcessing.Contracts - This project contains contracts that had to be shared between projects.
- LondonStockExnchange.DataProcessing.Write.Service - This project contains logic that allow us to write data received by LondonStockExchange.
- LondonStockExchange.DataProcessing.Read.Api - This project contains an API which is used to expose the last prices received by LondonStockExchange.

#How To Run:
- Pre-requisites:
You will need a SQL server installed and before trying to run the system you should run a SQL script which will setup the database that is being used by the system.

- You shall start:
1 - LondonStockExchange.DataProducer - This service will simulate a LondonStockExchange Feed.
2 - LondonStockExnchange.DataProcessing.Write.Service - This service will consume the LondonStockExchange Feed.
3 - LondonStockExchange.DataProcessing.Read.Api - This service will expose last prices for stocks received.


#Considerations 
##System Architecture
The system architecture is composed of:
A queue which can deal with high amount of messages comming into the system.
A set of Write Services which main purpose is to write the data from the queue into the database.
A set of Read Services which main purpose if to read the data that was written into the database.

The system is using CQRS which essentialy means we have separate the part of the system that deals with writes and read. This was done because the system needs to deal with high volumes of messages comming into the system and as I was not sure about the amount of read I thought it would be better to separate them to avoid having issue in case there is a high number of reads as well.

The system design shows the use of Primary/Replica database, but this was not implemented as this is more of a infrastructure consideration. So we have a single database from which we read and write data.
![image](https://user-images.githubusercontent.com/10722526/154343582-3d137be7-3b85-43cc-95e5-aa50d25e0b8c.png)

###Coding Decisions That Impact System Throughtput:
- Services writing to the database do not update data they are insert only services. This increases the costs of the database storage and may reduce system performance on the long run but reduces the problems concurrency specially which could slow down the system if we decide to have multiple replicas of the writing service running in parallel. To minimize the effects of this we could have in place a business process which would move part of the data that wouldn't be used to a data warehouse.

