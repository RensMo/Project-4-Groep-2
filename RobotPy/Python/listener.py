import socket
import sys
import socket

def startListen():
    host = 'localhost'
    port = 23023

    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    #sock.bind((host, port))

    message = "Je moeder"
    sock.sendto(message.encode(), (host, port))

    while True:
        data, addr = sock.recvfrom(1024)
        if not data:
            break

        print('From server: ' + data.decode())


    sock.close


if __name__ == "__main__":
    print("Listening")
    startListen()