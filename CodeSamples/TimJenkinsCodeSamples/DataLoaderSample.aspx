<%@ Page Title="Data Loader-Processor Queue" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DataLoaderSample.aspx.cs" Inherits="TimJenkinsCodeSamples.DataLoaderSample" %>

<asp:Content ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
                <h2>&nbsp;</h2>
            </hgroup>
            <p>
                This is a multi-threaded approach to a data loading/processing queue.  This encapsulates a 3-step process: 1) Read a data element, 2) Process the data element, resulting in one or more results, 3) Persist the results.  In this implementation, one thread reads data and puts it into an input queue, several threads process the data and put results into an output queue, and another thread persists the results to some data store.</p>
            <p>
                Originally, this was used to improve performance of a data processing flow that consisted of reading data from a file, performing complex processing operations, and persisting results to a database.  It started out as a serial process where we read batches of data rows from the file, did the processing, then persisted to the database, then back to file reading, and so on.  Because we were going back and forth between file io-bound operations, cpu-bound operations, and sql io-bound operations, at any given time two of the three resources were sitting idle.  The solution was to move them onto separate threads so that all three could be utilized at the same time.  Because the processing portion was a larger chunk of the time, that got parallelized into multiple processing threads.  The end result was a significant boost in throughput.</p>
            <h4>Instructions:</h4>
            <p>
                There is no demonstration to run for this sample, only code.</p>
            <h4>Code Files:</h4>
            <ul>
                <li><b>DataLoaderSample.aspx:</b> This web page.</li>
                <li><b>DataLoader.cs:</b> This is the thing that drives the process.  The constructor takes an IReader, IProcessor, and IWriter.  The Load method kicks it off.</li>
                <li><b>IProcessor.cs:</b> A simple interface for processing a data element to produce one or more results.</li>
                <li><b>IReader.cs:</b> A simple interface for reading a data element from some data store.</li>
                <li><b>IWriter.cs:</b> A simple interface for writing a data element to some data store.</li>
                <li><b>MockProcessor.cs:</b> A simple implementation of IProcessor that just waits a short time and returns some results.  This was only used for testing.</li>
                <li><b>MockReader.cs:</b> A simple implementation of IReader that just waits a short time and returns an empty string. This was only used for testing.</li>
                <li><b>MockWriter.cs:</b> A simple implementation of IWriter that just waits a short time and returns.  This was only used for testing.</li>
                <li><b>ProcessQueue.cs:</b> A wrapper around the ConcurrentQueue to provide a mechanism for marking the queue as Finished, meaning that no more data elements can be added.</li>
                <li><b>ThreadSafeRandom.cs:</b> A thread-safe wrapper around the Random class (which does not behave well in a multi-threaded scenario).  This is not immediately relevant to the problem at hand, but was used during testing.</li>
            </ul>
        </div>
    </section>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
