<%@ Page Title="ADO.NET Data Layer Abstraction" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdoDataLayerAbstraction.aspx.cs" Inherits="TimJenkinsCodeSamples.AdoDataLayerAbstraction" %>

<asp:Content ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
                <h2>&nbsp;</h2>
            </hgroup>
            <p>
                This code sample was an abstraction of ADO.NET data access layer that I wrote for a project where we had a large number of objects across many libraries and needed flexibility to persist/read to/from a variety of different flavors of SQL, like Microsoft SQL Server, MySQL, SQLite and SQLCompact.  This abstracts a lot of the underlying data access calls (the basic CRUD methods) and also provides a relatively easy means of adding customized data access methods as required.</p>
            <p>
                The core is the DbBaseData abstract class.  It provides a number of properties for defining the table structure as well as a number of methods for performing basic CRUD operations.  This class is written to be type-agnostic and performs translation between the code objects and the table columns (and vice versa) using delegate methods supplied by the implementation. The keywords and statements are based on T-SQL, but other variants of SQL can be supported by inheriting and overriding the necessary properties or methods.</p>
            <p>
                Also included here are a number of classes to facilitate retry mechanisms on pretty much any operation.  In particular, the DbRetryer is used extensively.  The retry model allows a wide range of flexible retry options, including waiting between tries, graduated wait times, blacklisted exceptions (if one of these happens, do not retry), and extension methods for several ADO.NET interfaces.</p>
            <p>
                Last, there is a sample implementation that demonstrates how this data layer abstraction is used.  In a nutshell, there are a few code objects and a data factory.  For each object, there is an interface defining the specific data access operations provided, and those are tied together through an abstract data factory.  For each specific implementation, there is a set of classes...one providing a concrete implementation of the data factory and also one implementing each object's data access interface.  All SQL-like data access could be handled by a single implementation thanks to the flexibility of DbBaseData, but you could also have other implementations that are not encapsulated by DbBaseData, such as XML, flat files, or a no-SQL solution like MongoDB. </p>
            <h4>Instructions:</h4>
            <p>
                There is no demonstration to run for this sample, only code.</p>
            <h4>Code Files:</h4>
            <ul>
                <li><b>AdoDataLayerAbstraction.aspx:</b> This web page.</li>
                <li><b>DataAbstraction\DataContext.cs:</b> Abstract class representing a data context.</li>
                <li><b>DataAbstraction\DataProviderType.cs:</b> Enum listing several different data providers with ADO.NET support.</li>
                <li><b>DataAbstraction\DbBaseData.cs:</b> Abstract class encapsulating a number of properties for describing a data table and a number of methods for accessing the data.</li>
                <li><b>DataAbstraction\DbHelper.cs:</b> Static class containing a number of methods to perform common database and related operations.</li>
                <li><b>DataAbstraction\DefaultValues.cs:</b> Static class containing several default value constants.</li>
                <li><b>DataAbstraction\IdSequenceTableInfo.cs:</b> Class used by DbHelper to assist in dealing with tables that track identity values.  This was a concept that was prevalent througout a lot of legacy databases we had to deal with, where instead of using auto-increment identities, there were separate tables that tracked the next available id value.</li>
                <li><b>DataAbstraction\SqlDataContext.cs:</b> Concrete implementation of DataContext for SQL databases.</li>
                <li><b>Retry\DbRetryer.cs:</b> Standard retryer for database operations.</li>
                <li><b>Retry\ExecuteReaderRetryer.cs:</b> Retryer specifically tailored for IDbCommand.ExecuteReader.</li>
                <li><b>Retry\GraduatedWaitTimeIncrementalRetryer.cs:</b> Retryer that waits between tries, and the wait time increases each time.</li>
                <li><b>Retry\IDbRetryCommand.cs:</b> Interface that decorates IDbCommand with a DbRetryer.</li>
                <li><b>Retry\IDbRetryConnection.cs:</b> Interface that decorates IDbConnection with a DbRetryer.</li>
                <li><b>Retry\IncrementalRetryer.cs:</b> Retryer that will retry up to a specific number of times.</li>
                <li><b>Retry\IORetryer.cs:</b> Standard Retryer for file operations.</li>
                <li><b>Retry\Retry.cs:</b> Abstract base class for all retryers.</li>
                <li><b>Retry\RetryCommandExtensions.cs:</b> Extension methods for IDbRetryCommand to add retry functionality to various Command operations.</li>
                <li><b>Retry\RetryConnectionExtensions.cs:</b> Extension methods for IDbRetryConnection to add retry functionality to various Connection operations.</li>
                <li><b>Retry\Retryer.cs:</b> A static class that provides access to use WaitTimeIncrementalRetryer without having to create your own instance and also returns RetryerResults.</li>
                <li><b>Retry\RetryerResults.cs:</b> Encapsulates the results of a retry operation performed by Retryer.  It contains the underlying Retry object and any exceptions that were produced in the process.</li>
                <li><b>Retry\RetryLogExtensions.cs:</b> Extension methods for IDbCommand and IDbConnection to log on retries.</li>
                <li><b>Retry\RetryOptions.cs:</b> Encapsulates various options for controlling retry behavior.</li>
                <li><b>Retry\WaitTimeIncrementalRetryer.cs:</b> Retryer that waits a set time between tries.</li>
                <li><b>Retry\WaitTimeIncrementDelegate.cs:</b> Defines a delegate for determining the wait time between tries.</li>
                <li><b>Sample\SomeObject.cs:</b> A simple object used for demonstrating the implementation.</li>
                <li><b>Sample\SomeChildObject.cs:</b> A simple object used for demonstrating the implementation.</li>
                <li><b>Sample\DataLayer\StuffDataFactory.cs:</b> Abstract data factory representing data access for our code objects.</li>
                <li><b>Sample\DataLayer\ISomeObjectData.cs:</b> Interface defining supported data access methods for SomeObject.</li>
                <li><b>Sample\DataLayer\ISomeChildObjectData.cs:</b> Interface defining supported data access methods for SomeChildObject.</li>
                <li><b>Sample\DataLayer\Db\DbStuffDataFactory.cs:</b> Concrete implementation of StuffDataFactory for use with SQL-like data storage.</li>
                <li><b>Sample\DataLayer\Db\DbSomeObjectData.cs:</b> Implementation of ISomeObjectData for SQL-like data storage.</li>
                <li><b>Sample\DataLayer\Db\DbSomeChildObjectData.cs:</b> Implementation of ISomeChildObjectData for SQL-like data storage.</li>
            </ul>
        </div>
    </section>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
