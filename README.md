# LondonStockExchange
This solution contains five different projects:
- LondonStockExchange.DataProducer - Opens an Swagger page which can be used to ingest data into the system.
- LondonStockExchange.DataProcessing.Tests - This project contains test for all projects in the system.
- LondonStockExchange.DataProcessing.Contracts - This project contains contracts that had to be shared between projects.
- LondonStockExnchange.DataProcessing.Write.Service - This project contains logic that allow us to write data received by LondonStockExchange.
- LondonStockExchange.DataProcessing.Read.Api - This project contains an API which is used to expose the last prices received by LondonStockExchange.

# How To Run:
- Pre-requisites:
  - You will need a SQL server installed (I used https://www.microsoft.com/en-gb/sql-server/sql-server-downloads).
  - After installing the SQL Server run the SQL scripts found in /SQL folder to setup the database and its tables. You will endup with something as follows (no data in the table!):
![image](https://user-images.githubusercontent.com/10722526/154356309-89bab495-1687-4779-b098-07a1495d5510.png)
  - The connection strings in all projects that require database access use SSPI authentication due to this there is no need to do any changes in the projects. If you still decide to change them connection string can be modified at appSettings.Development.json

- You need to start the following services:
  - 1 - LondonStockExchange.DataProducer - This service will simulate a LondonStockExchange Feed (Produces 10 Transactions Per Request).
  - 2 - LondonStockExnchange.DataProcessing.Write.Service - This service will consume the LondonStockExchange Feed.
  - 3 - LondonStockExchange.DataProcessing.Read.Api - This service will expose last prices for stocks received.
![image](https://user-images.githubusercontent.com/10722526/154355778-c439de38-c45e-45a1-90ba-a28f7b222e74.png)

# Considerations 
- In the upcoming sections I discuss some of the decisions took.

## Latest System Architecture V2 (Focus On Processing)
The system architecture is composed of:
- A queue which can deal with high volumes of messages comming into the system.
- A Write Service which main purpose is to write the data from the queue into the database.
- A Read Service which main purpose if to read the data from the database.

The system uses some of the concepts of "CQRS" which essentialy means we have separated the part of the system that deals with writes and read. This was done because the system needs to deal with high volumes of messages comming in, as I was not sure on the amount of reads I thought it would be better to separate them to avoid having issues in case there is a high number of reads as well.

The system design shows the use of Primary/Replica database, but this was not implemented as this is more of a infrastructure consideration. I was not sure about the consistency level of the system. One higly scalable system with strong consistency is very hard to acheive so I assumed that the system would be "eventually consistent" (or have some delay) and decided to use database replicas. The primary database is used for writes, and the replica used for reads. 

![image](https://user-images.githubusercontent.com/10722526/154353210-28f1fe04-3462-4f4e-8071-5016bafdcb55.png)

### Bottlenecks in Architecture
- Queue may be a bottleneck they introduce resilience into the system and reduce problems with requests timing out, but they introduce latency as services may not be able to coupe with the amount of data being ingested.
- Database we have a single instance of the database which is used to write and read data, this is essentialy slowing down the overall performance of the system. Even with the implementation highlighed in the architecture where we would have a Primary and Replicate database we could still have problems when writing if the volumes are really high.

### Coding Decisions That Impact System Throughtput:
- Services writing to the database do not update data they are insert only services. This increases the costs of the database storage and may reduce system performance on the long run (data will grow) but reduces the  concurrency problems which could slow down the system if we decide to have multiple replicas of the writing service running in parallel. To minimize the effects of this we could have in place a business process which would move part of the data that wouldn't be used to a data warehouse for example.

### Coding Decisions And Librarires
Why NServiceBus? 
- I decided to use NServiceBus as this facilitates the simulation of queues and that was the main reason why I decided to use it. Currently I am using a Learning Transport but obviously this would be using Azure Service Bus or other queuing system.

Why TradeDateTime in Transaction model?
- This is the time of the trade was placed and will allow us to infer what was the last trade for a given ticker. This help was our system is insert only system and there are no updates (optimistic or pessimistic concurrency handling).

Why not using DDD?
- DDD and microservice get along really well as DDD helps to do functional boundaries of the system. The system that I was asked to build is not heavy in regards to business logic for this reason there was no need to apply any DDD.

# Initial System Architecture V1 (**This architecture is not the one the system is reflecting**)
The system architecture went though various changes. Initialy I considered the part of the system that deals ingestion of data. As I was not sure how brokers would be sending us data the system was designed so that:
- They could interact through Ingestion API which would be hidden under a LoadBalancer to allow for high volume of requests. 
- They could interact through the queue that could be acessible to them.
During this phase of the system deisgn I was also considering to have Writing and Reading functionalities living alongside in process. These would be reading and writing from the same database.
![image](https://user-images.githubusercontent.com/10722526/154350689-f13fd40c-e667-49fa-bbed-2610c0813beb.png)
