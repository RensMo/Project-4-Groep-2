import socket
import sys
import threading
import json

class UdpHandler:
    host = ''
    port = 0
    sock = None
    listening = False
    l_thread = False

    def __init__(self, hostport):
        self.host = 'localhost'
        self.port = hostport
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

    def send(self, message):
        self.sock.sendto(message.encode(), (self.host, self.port))

    def startlistening(self):
        if (self.listening == False):
            self.listening = True
            self.l_thread = threading.Thread(target=self.listen())
            self.l_thread.daemon = True
            self.l_thread.start()

    def stoplistening(self):
        self.listening = False

    #Only use 'stoplistening()' and 'startlistening()' please.
    def listen(self):
        while(self.listening):
            data, addr = self.sock.recvfrom(1024)
            if not data:
                break
            print('From server: ' + data.decode())


if __name__ == "__main__":
    udpman = UdpHandler(23023)
    udpman.send("Hoi, ik ben udpman")
    udpman.startlistening()