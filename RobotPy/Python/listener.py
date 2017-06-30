import socket
import sys
import pygame
import json
import threading
from threading import Thread

pygame.init()
screen = pygame.display.set_mode((800, 480))
screen.fill((255,255,255))
pygame.display.flip()


# load some images
myfont = pygame.font.SysFont("Arial", 15)
MainCharacterImage = pygame.image.load("MainCharacter.png")
EnemyCharacterImage = pygame.image.load("EnemyCharacter.png")
whitepixel = pygame.image.load("white_pixel.png")


class jsontofunction:
    def __init__(self, string):
        self.string = string

    def json(self, data):
        self.string = data


jsontofunction = jsontofunction("")


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
            Thread(target=self.listen).start()

    def stoplistening(self):
        self.listening = False

    #Only use 'stoplistening()' and 'startlistening()' please.
    def listen(self):
        while(self.listening):
            data, addr = self.sock.recvfrom(1024)
            if not data:
                break
            print(data.decode())
            string = json.loads(data.decode())
            print(string["args"][2])
            if string["func"] == "drawRectangle":
                X = string["args"][1]
                X = int(X)
                Y = string["args"][2]
                Y = int(Y)
                z = X + Y
                print(z)
                screen.blit(whitepixel, (X, Y))
            if string["func"] == "drawMainCharacter":
                X = string["args"][0]
                X = int(X)
                Y = string["args"][1]
                Y = int(Y)
                z = X + Y
                print(z)
                screen.blit(MainCharacterImage, (X, Y))
            pygame.display.update()


udphandler = u=UdpHandler(23023)
running = True
udphandler.startlistening()

while running:
    for event in pygame.event.get():
        #if event.type == pygame.QUIT:
        #   running = False
        print("hello-----------------------------------------------------------------------------------")

        if event.type == pygame.KEYDOWN:
            pressedkeys = pygame.key.get_pressed()
            returnlist = ""

            if pressedkeys[pygame.K_a]:
                # add key a logic
                returnlist += "A,"

            if pressedkeys[pygame.K_w]:
                # add key w logic
                returnlist +="W,"

            if pressedkeys[pygame.K_s]:
                # add key s logic
                returnlist += "S,"

            if pressedkeys[pygame.K_d]:
                # add key d logic
                returnlist += "D,"

            if pressedkeys[pygame.K_RIGHT]:
                if pressedkeys[pygame.K_UP]:
                    returnlist += "UpRight,"
                elif pressedkeys[pygame.K_DOWN]:
                    returnlist += "DownRight,"
                else:
                    returnlist += "Right,"

            if pressedkeys[pygame.K_LEFT]:
                if pressedkeys[pygame.K_UP]:
                    returnlist += "UpLeft,"
                elif pressedkeys[pygame.K_DOWN]:
                    returnlist += "DownLeft,"
                else:
                    returnlist += "Left,"

            if pressedkeys[pygame.K_UP]:
                if pressedkeys[pygame.K_RIGHT]:
                    returnlist += "UpRight,"
                elif pressedkeys[pygame.K_LEFT]:
                    returnlist += "UpLeft,"
                else:
                    returnlist += "Up,"

            if pressedkeys[pygame.K_DOWN]:
                if pressedkeys[pygame.K_RIGHT]:
                    returnlist += "DownRight,"
                elif pressedkeys[pygame.K_LEFT]:
                    returnlist += "DownLeft,"
                else:
                    returnlist += "Down,"
            udphandler.send(returnlist)
    screen.fill([0,0,0])
    udphandler.send("hey")

    screen.blit(EnemyCharacterImage, (0,0))
    pygame.display.update()

    # check which function is used

    #if "drawText" in rawdatastring:
    #   rawdatastring = rawdatastring.replace("{", "")
    #   rawdatastring = rawdatastring.replace("}", "")
    #   rawdatastring = rawdatastring.replace(":", "")
    #   rawdatastring = rawdatastring.replace("func", "")
    #   rawdatastring = rawdatastring.replace("'", "")
    #   rawdatastring = rawdatastring.replace("[", "")
    #   rawdatastring = rawdatastring.replace("]", "")
    #   rawdatastring = rawdatastring.replace("args", "")
    #   rawdatastring = rawdatastring.split(",")
    #   label = myfont.render(rawdatastring[1], 1, (0,0,0))
    #   screen.blit(label, (int(rawdatastring[3]))(int(rawdatastring[4])))
    #if "drawRectengle" in rawdatastring:


    #screen.blit(screen, )

    # if received input from c# app




