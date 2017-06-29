import socket
import sys

# The C# side
# Receiving key press input.
# Sending drawing objects


def startA():
    host = 'localhost'
    port = 23023

    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    sock.bind((host, port))
    i = 0

    while True:
        print(i)
        i = i + 1
        data, addr = sock.recvfrom(1024)
        if not data:
            break
        print("From connected user: " + data.decode())

        data = input(" ? ")
        sock.sendto(data.encode(), addr)

    #sock.close


if __name__ == "__main__":
    print("A")
    startA()