# server-python2.py
import socket
import cv2
import numpy as np
#from sense_hat import SenseHat

host = "192.168.XX.XX " # server IP
port = XXXX # port number

server_sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_sock.bind((host,port))
server_sock.listen(5)

width = 400
height = 300

wide = width // 6  # triangle_width
high = height // 5  # triangle_height

x = width // 2
y1 = height // 2 - height // 6 #50
y2 = height - height // 3 #100

y = height // 2
x1l = width // 2 - width // 20
x2l = width // 2 + width // 20 + high
x1r = width // 2 - width // 20 - high
x2r = width // 2 + width // 20


def run_display():
    # window
    cv2.namedWindow('Display', cv2.WINDOW_NORMAL)
    cv2.setWindowProperty('Display', cv2.WND_PROP_FULLSCREEN, cv2.WINDOW_FULLSCREEN)

    # Loading Images
    image_path1 = 'img/mark_batsu.png'
    image_path2 = 'img/mark_sos.png'  
    image1 = cv2.imread(image_path1)
    image2 = cv2.imread(image_path2)

    while True:
        msg = client_sock.recv(1024).decode()
        print("受信: ", msg)

        if (msg == "batu"): #×の表示
            img = image1.copy()

        elif (msg == "SOS"): #SOSの表示
            img = image2.copy()
        
        elif (msg == "q"): #終了判定
            break

        else: # 矢印の描画
            img = np.zeros((height, width, 3), np.uint8)

            if (msg == "non"):
                points1 = np.array([[x, y1], [x - wide, y1 - high], [x + wide, y1 - high]]).reshape(1, -1, 2)
                points2 = np.array([[x, y2], [x - wide, y2 - high], [x + wide, y2 - high]]).reshape(1, -1, 2)
            
            elif (msg == "left"):
                points1 = np.array([[x1r, y], [x1r + high, y + wide], [x1r + high, y - wide]]).reshape(1, -1, 2)
                points2 = np.array([[x2r, y], [x2r + high, y + wide], [x2r + high, y - wide]]).reshape(1, -1, 2)
            
            elif (msg == "right"):
                points1 = np.array([[x1l, y], [x1l - high, y + wide], [x1l - high, y - wide]]).reshape(1, -1, 2)
                points2 = np.array([[x2l, y], [x2l - high, y + wide], [x2l - high, y - wide]]).reshape(1, -1, 2)

            cv2.fillPoly(img, points1, color=(0, 255, 0))
            cv2.fillPoly(img, points2, color=(0, 255, 0))
            
        # show display
        cv2.imshow('Display', img)

        # judge ending
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

    cv2.destroyAllWindows()
    client_sock.close() # close socket

if __name__ == '__main__':

    print("waiting client...")
    (client_sock, client_addr) = server_sock.accept()

    print("accept client: ", client_addr)
    run_display()
 