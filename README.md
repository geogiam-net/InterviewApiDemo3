# Introduction 
This a small Demo of a web api project to show my skills as programmer and knowledge in architecture.

# Getting Started
What you need to run the project and open resources:
1.	Install .Net 10
2.	Install Visual Studio 2026
3.	Install draw.io to open the diagram document 'Demo Diagram.drawio'

# Architecture and Diagram explanation

After reading the Requirement document 'Technical Assignment.docx' from the business analyst, the 1st thing done was to abstract the problem up, from the origin level, from there, one analyzes and break the problem downwards.

That is how we ended with the diagrams 'Problem', 'Domain', which explain in one sentence what is needed.

Both diagrams are important because one can identify that there are 2 Domains. It is valid to have the domain coupled or separated, it depends of the situation, depending of that the architecture downwards changes.

For example, 'Employee Management' exists in an external paid software and 'Shift Management' is the new system one wants to program. Or it could be wished that 'Employee Management' and 'Shift Management' remain independant form each other, different databases, unique web api instances (and escalation) for each other.

In this Demo, one places them together for simplicity and because there is no reason not to, the requirements are short and simple.

With that in mind, we model the domain by using the ubiquituos language, so we end up with diagram 'Domain and System Layers (simple)'.

This diagram explains the parts of the system that are required, the user cases that our Business Software will solve/handle and finally how we set up the dependencies.

The last diagram, 'Domain and System Layers (detailed)', is a technical diagram, it transforms the previous diagram into an actual programming interface.

This diagram displays the interfaces created to serve the use cases, also how we model the business logic, persist the data, separate the and connect the layers of the software, finally infrastructure that is used.

# Assumptions

It was considered, that with only the properties from the requirements, a instance shift would be hard to identify for a user. Its purpose will not be clear. Also there is no restriction in place to configure multiple shifts at the same time.
Because of that, it was added a 'Name' property, so users can named it or short describe it.

There is no requirement to get a shift with an Id, but it was considered important to add the use case for consistency and testing purposes.

# For production

In a real live scenario, protecting private and business data is priority, one will need a security system where users could login before accessing the endpoints.
So the access to resources has to be protected with Authentication and Authorization, each endpoint muss expect a token and it will be handle in the middleware of the pipeline.
One needs to define the users, roles and permissions and importantly think how to create/modify/store them.
Perhaps a small web page or web api can be created so admins can set/change permissions with ease.
In any case, the security system needs his own database and app instances, it has to separated and scale independently from the business.
And out of the box solution is to use Microsoft.AspNetCore.Identity.

In the Demo we use an In-Memory database, this will have to be replaced for a non volatile storage, like SqlServer for example.
What is needed is to change the dependency injection inside Program.cs, create a migration in the 'Demo.Infrastructure.SqlStorage', and at last, prodive the connection string, which should be stored securely somewhere and encrypted, (never in the same repository), it could be in Azure as App parameter for example.
Depending on costs and other brand performance, other databases can be used instead.

If there are high concurrency scenarios, the best is to break the system as much as possible so one can scale the parts that are used the most and require more processing power/time.
Perhaps, it would make sense to decouple 'Employee Management' from 'Shift Management', perhaps shifts are queried constantly but employees change only once a day.
I am not an expert in optimizations, but of course, having multiple instances of databases, webservices and cache, like Reddis, would help.
