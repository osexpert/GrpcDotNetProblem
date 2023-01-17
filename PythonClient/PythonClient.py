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
"""The Python implementation of the gRPC route guide client."""

from __future__ import print_function

import logging
import random

import grpc

import greet_pb2
import greet_pb2_grpc
# import gree_resources


def make_request(mess):
    return greet_pb2.EchoRequest(
        request=mess
        )


def generate_messages(count):
    messages = [
        make_request("First message" + str(count))
    ]
    for msg in messages:
        print("Sending " + msg.request)
        yield msg


def echo_test(stub, count):
    responses = stub.EchoBidir(generate_messages(count))
    for response in responses:
        print("reply test" + str(count) + " " + response.reply)


def echo_test_hangup(stub, count):
    responses = stub.EchoBidirHangup(generate_messages(count))
    for response in responses:
        if response.reply == 'quit':
            break;
        print("reply hangup test" + str(count) + " " + response.reply)

def run():
    # NOTE(gRPC Python Team): .close() is possible on a channel and should be
    # used in circumstances in which the with statement does not fit the needs
    # of the code.
    with grpc.insecure_channel('localhost:5000') as channel:
        stub = greet_pb2_grpc.GreeterStub(channel)

        count = 0

        while True:
            #echo_test(stub, count)
            echo_test_hangup(stub, count)
            count = count + 1

        
        

if __name__ == '__main__':
    logging.basicConfig()
    run()
