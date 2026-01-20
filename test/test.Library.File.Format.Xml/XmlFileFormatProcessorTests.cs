using Library.File.Core;
using Library.File.Core.Format;
using Library.File.Format.Xml;

using Microsoft.Extensions.DependencyInjection;

using test.Common;

namespace test.Library.File.Format.Xml;


public sealed class XmlFileFormatProcessorTests
{
    [Fact]
    public void Read_ShouldThrowArgumentNullException_WhenStreamIsNull()
    {
        IServiceCollection services = new ServiceCollection();
        FileDependenciesRegistrationsOptions.Configure(
            services, 
            s => s.AddFileFormatProcessor(opts => opts.UseXmlFileFormatProcessor()));

        // arrange
        var sut = services.BuildServiceProvider().GetRequiredService<IFileFormatProcessor<XmlFileFormatType>>();

        // act
        var act = Record.Exception(() => sut.Read<TestRecord>(null!));

        // assert
        act.Should().NotBeNull();
    }

    [Fact]
    public void Read_ShouldThrowArgumentException_WhenStreamCannotRead()
    {
        IServiceCollection services = new ServiceCollection();
        FileDependenciesRegistrationsOptions.Configure(
            services, 
            s => s.AddFileFormatProcessor(opts => opts.UseXmlFileFormatProcessor()));


        // arrange
        var sut = services.BuildServiceProvider().GetRequiredService<IFileFormatProcessor<XmlFileFormatType>>();
        using var stream = new NonReadableStream();

        // act
        var act = Record.Exception(() => sut.Read<TestRecord>(stream).ToList());

        // assert
        act.Should().NotBeNull();
    }

    [Fact]
    public void Read_ShouldReturnEmpty_WhenSeekableStreamLengthIsZero()
    {
        // arrange
        IServiceCollection services = new ServiceCollection();
        FileDependenciesRegistrationsOptions.Configure(
            services, 
            s => s.AddFileFormatProcessor(opts => opts.UseXmlFileFormatProcessor()));

        // arrange
        var sut = services.BuildServiceProvider().GetRequiredService<IFileFormatProcessor<XmlFileFormatType>>();
        
        using var stream = new MemoryStream();

        // act
        var result = sut.Read<TestRecord>(stream);

        // assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Read_ShouldResetPositionToZero_WhenStreamIsSeekable()
    {
        // arrange
        IServiceCollection services = new ServiceCollection();
        FileDependenciesRegistrationsOptions.Configure(
            services, 
            s => s.AddFileFormatProcessor(opts => opts.UseXmlFileFormatProcessor()));

        // arrange
        var sut = services.BuildServiceProvider().GetRequiredService<IFileFormatProcessor<XmlFileFormatType>>();

        var xml = """
                  <?xml version="1.0" encoding="utf-8"?>
                  <records>
                    <record>
                      <Id>1</Id>
                      <Name>Alpha</Name>
                    </record>
                    <record>
                      <Id>2</Id>
                      <Name>Beta</Name>
                    </record>
                  </records>
                  """;

        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));

        stream.Position = stream.Length;

        // act
        var result = sut.Read<TestRecord>(stream).ToList();

        // assert
        result.Should().HaveCount(2);
        result[0]!.Id.Should().Be(1);
        result[0]!.Name.Should().Be("Alpha");
        result[1]!.Id.Should().Be(2);
        result[1]!.Name.Should().Be("Beta");
    }

    [Fact]
    public void Read_ShouldThrowInvalidOperationException_WhenXmlIsMalformed()
    {
        // arrange
        IServiceCollection services = new ServiceCollection();
        FileDependenciesRegistrationsOptions.Configure(
            services, 
            s => s.AddFileFormatProcessor(opts => opts.UseXmlFileFormatProcessor()));

        // arrange
        var sut = services.BuildServiceProvider().GetRequiredService<IFileFormatProcessor<XmlFileFormatType>>();

        var xml = "<records><record><Id>1</Id></record>"; // invalid xml
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));

        // act
        var act = Record.Exception(() => sut.Read<TestRecord>(stream).ToList());

        // assert
        act.Should().NotBeNull();
    }

    [Fact]
    public void Write_ShouldThrowArgumentNullException_WhenStreamIsNull()
    {
        // arrange
        IServiceCollection services = new ServiceCollection();
        FileDependenciesRegistrationsOptions.Configure(
            services, 
            s => s.AddFileFormatProcessor(opts => opts.UseXmlFileFormatProcessor()));

        // arrange
        var sut = services.BuildServiceProvider().GetRequiredService<IFileFormatProcessor<XmlFileFormatType>>();

        // act
        var act = Record.Exception(() => sut.Write(null!, [new TestRecord { Id = 1, Name = "A" }]));

        // assert
        act.Should().NotBeNull();
    }

    [Fact]
    public void Write_ShouldThrowArgumentNullException_WhenRecordsIsNull()
    {
        // arrange
        IServiceCollection services = new ServiceCollection();
        FileDependenciesRegistrationsOptions.Configure(
            services, 
            s => s.AddFileFormatProcessor(opts => opts.UseXmlFileFormatProcessor()));


        // arrange
        var sut = services.BuildServiceProvider().GetRequiredService<IFileFormatProcessor<XmlFileFormatType>>();

        using var stream = new MemoryStream();

        // act
        var act = Record.Exception(() => sut.Write<TestRecord>(stream, null!));

        // assert
        act.Should().NotBeNull();
    }

    [Fact]
    public void Write_ShouldThrowArgumentException_WhenStreamCannotWrite()
    {
        // arrange
        IServiceCollection services = new ServiceCollection();
        FileDependenciesRegistrationsOptions.Configure(
            services, 
            s => s.AddFileFormatProcessor(opts => opts.UseXmlFileFormatProcessor()));

        // arrange
        var sut = services.BuildServiceProvider().GetRequiredService<IFileFormatProcessor<XmlFileFormatType>>();

        using var stream = new NonWritableStream();

        // act
        var act = Record.Exception(() => sut.Write(stream, [new TestRecord { Id = 1, Name = "A" }]));

        // assert
        act.Should().NotBeNull();
    }

    [Fact]
    public void Write_ShouldClearAndResetPosition_WhenStreamIsSeekable()
    {
        // arrange
        IServiceCollection services = new ServiceCollection();
        FileDependenciesRegistrationsOptions.Configure(
            services, 
            s => s.AddFileFormatProcessor(opts => opts.UseXmlFileFormatProcessor()));

        // arrange
        var sut = services.BuildServiceProvider().GetRequiredService<IFileFormatProcessor<XmlFileFormatType>>();

        using var stream = new MemoryStream();

        using (var sw = new StreamWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            sw.Write("garbage");
            sw.Flush();
        }

        stream.Position = stream.Length;

        var records = new[]
        {
            new TestRecord { Id = 10, Name = "Ten" }
        };

        // act
        sut.Write(stream, records);

        // assert
        stream.Position.Should().Be(0);

        var xmlText = ReadAllText(stream);
        xmlText.Should().Contain("<records>");
        xmlText.Should().Contain("<record>");
        xmlText.Should().Contain("<Id>10</Id>");
        xmlText.Should().Contain("<Name>Ten</Name>");
    }

    [Fact]
    public void Write_ShouldNotAddXsiXsdNamespaces()
    {
        // arrange
        IServiceCollection services = new ServiceCollection();
        FileDependenciesRegistrationsOptions.Configure(
            services, 
            s => s.AddFileFormatProcessor(opts => opts.UseXmlFileFormatProcessor()));

        // arrange
        var sut = services.BuildServiceProvider().GetRequiredService<IFileFormatProcessor<XmlFileFormatType>>();

        using var stream = new MemoryStream();

        // act
        sut.Write(stream, [new TestRecord { Id = 1, Name = "A" }]);

        // assert
        var xmlText = ReadAllText(stream);
        xmlText.Should().NotContain("xmlns:xsi");
        xmlText.Should().NotContain("xmlns:xsd");
    }

    [Fact]
    public void Write_ThenRead_ShouldRoundtripRecords()
    {
        // arrange
        IServiceCollection services = new ServiceCollection();
        FileDependenciesRegistrationsOptions.Configure(
            services, 
            s => s.AddFileFormatProcessor(opts => opts.UseXmlFileFormatProcessor()));

        // arrange
        var sut = services.BuildServiceProvider().GetRequiredService<IFileFormatProcessor<XmlFileFormatType>>();

        using var stream = new MemoryStream();

        var input = new List<TestRecord>
        {
            new() { Id = 1, Name = "Alpha" },
            new() { Id = 2, Name = "Beta" },
        };

        // act
        sut.Write(stream, input);
        var output = sut.Read<TestRecord>(stream).ToList();

        // assert
        output.Should().HaveCount(2);
        output.Select(x => x!.Id).Should().Equal(1, 2);
        output.Select(x => x!.Name).Should().Equal("Alpha", "Beta");
    }

    private static string ReadAllText(Stream stream)
    {
        if (stream.CanSeek) stream.Position = 0;
        using var sr = new StreamReader(stream, System.Text.Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: true);
        var text = sr.ReadToEnd();
        if (stream.CanSeek) stream.Position = 0;
        return text;
    }

    public sealed class TestRecord
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}