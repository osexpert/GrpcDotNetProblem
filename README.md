Bug: https://github.com/grpc/grpc-dotnet/issues/2010

When running PhytonClient -> grpc-dotnetServer via VS itself it works, but when running both directly on command line, it fails.

Running GrpcCoreClient -> grpc-dotnetServer fails even when run via VS.

I guess when running via VS (or when enabling logging), it does not fail (as easy) as things will generally run slower so grpc-dotnetServer will get time to clean up the leaking resources, so the bug will not manifest as easily.
