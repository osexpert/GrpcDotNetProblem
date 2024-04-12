Bug: https://github.com/grpc/grpc-dotnet/issues/2010

When running PhytonClient via VS itself it works, but when running directly on command line, it fails.
Similar problem with GrpcService. With logging, it does not fail (as easy) as it will generally slow down and have time to clean up the leaking resources, and prevent the error for manifesting.
