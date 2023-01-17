
# Copyright 2015 gRPC authors.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
"""The Python implementation of the gRPC route guide server."""

from concurrent import futures
import logging
import math
import time

import grpc
import greet_pb2
import greet_pb2_grpc
#import route_guide_resources



def make_reply(mess):
    return greet_pb2.EchoReply(
        reply=mess
        )



class GreetServicer(greet_pb2_grpc.GreeterServicer):

    def EchoBidir(self, request_iterator, context):

        msg =  request_iterator.next()
        print("echo test: " + msg.request)
        yield make_reply("echo: " + msg.request);

        # yield make_reply("quit");

    def EchoBidirHangup(self, request_iterator, context):

        msg =  request_iterator.next()
        print("echo hangup test: " + msg.request)
        yield make_reply("echo: " + msg.request);

        yield make_reply("quit");


def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    greet_pb2_grpc.add_GreeterServicer_to_server(
        GreetServicer(), server)
    server.add_insecure_port('[::]:5000')
    server.start()
    server.wait_for_termination()


if __name__ == '__main__':
    logging.basicConfig()
    serve()
