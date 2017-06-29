import socket
import sys

# The Python/Pygame side
# Sending key press input.
# Receiving drawing objects


def startB():
    host = 'localhost'
    port = 23023

    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

    message = input(" ? ")
    while message != 'q':
        sock.sendto(message.encode(), (host, port))
        data, addr = sock.recvfrom(1024)

        print('From server: ' + data.decode())

        message = input(" ? ")

    #sock.close


if __name__ == "__main__":
    print("B")
    startB()