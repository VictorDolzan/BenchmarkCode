using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using BenchmarkCode.Helpers;
using BenchmarkDotNet.Attributes;
using InvalidOperationException = Amazon.CloudWatchLogs.Model.InvalidOperationException;

namespace BenchmarkCode.Benchmarks;

public class AwsLogBenchmark
{
    private IAmazonCloudWatchLogs _client;
    private const string _logGroup = "TestGroup";
    private static readonly string _logStream = "2024120604";
        //DateHelper.GetBrasiliaTime().ToString("yyyyMMddHH");

    public AwsLogBenchmark()
    {
        if (!new CredentialProfileStoreChain().TryGetAWSCredentials("my-profile", out var credentials))
        {
            throw new InvalidOperationException("AWS credentials not found for the specified profile.");
        }

        _client = new AmazonCloudWatchLogsClient(
            credentials,
            new AmazonCloudWatchLogsConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.USEast1,
            });
    }

    [Benchmark]
    public async Task OriginalLogStream()
    {
        var request = new DescribeLogStreamsRequest
        {
            LogGroupName = _logGroup,
            LogStreamNamePrefix = _logStream
        };

        var response = await _client.DescribeLogStreamsAsync(request);
        var logStream = response.LogStreams.FirstOrDefault();

        if (logStream == null)
        {
            await _client.CreateLogStreamAsync(new CreateLogStreamRequest()
            {
                LogGroupName = _logGroup,
                LogStreamName = _logStream
            });
        }
    }

    [Benchmark]
    public async Task RefactoredLogStream()
    {
        var request = new DescribeLogStreamsRequest
        {
            LogGroupName = _logGroup,
            LogStreamNamePrefix = _logStream
        };

        DescribeLogStreamsResponse response;
        do
        {
            response = await _client.DescribeLogStreamsAsync(request);

            if (response.LogStreams.Any(ls => ls.LogStreamName.Equals(_logStream)))
            {
                return;
            }

            request.NextToken = response.NextToken;
        } while (response.NextToken != null);

        await _client.CreateLogStreamAsync(new CreateLogStreamRequest()
        {
            LogGroupName = _logGroup,
            LogStreamName = _logStream
        });
    }

    [Benchmark]
    public async Task OriginalLogGroup()
    {
        var request = new DescribeLogGroupsRequest();
        List<LogGroup> allLogGroups = new List<LogGroup>();

        do
        {
            var response = await _client.DescribeLogGroupsAsync(request);
            allLogGroups.AddRange(response.LogGroups);
            request.NextToken = response.NextToken;
        } while (request.NextToken != null);

        if (allLogGroups.Any(x => x.LogGroupName == _logGroup))
            return;

        _ = await _client.CreateLogGroupAsync(new CreateLogGroupRequest()
        {
            LogGroupName = _logGroup
        });
    }

    [Benchmark]
    public async Task RefactoredLogGroup()
    {
        var request = new DescribeLogGroupsRequest()
        {
            LogGroupNamePrefix = _logGroup
        };

        do
        {
            var response = await _client.DescribeLogGroupsAsync(request);

            if (response.LogGroups.Any(lg => lg.LogGroupName.Equals(_logGroup)))
            {
                return;
            }

            request.NextToken = response.NextToken;
        } while (request.NextToken != null);

        _ = await _client.CreateLogGroupAsync(new CreateLogGroupRequest()
        {
            LogGroupName = _logGroup
        });
    }
}