#import doplnujicich knihoven
import random as rndm #knihovna random pro mod, kdy hrac hraje proti pc - nahodne pole
import os #knihovna os, pouziva se pro postupne mazani terminalu
import time #knihovna time pro delay mezi jednotlivymi prechody hry

#trida obsahujici nutne funkce pro spravny beh hry
class Game: 
    def __init__(self, mode=None, player1=None, player2=None): #inicializace predstaveni a nastaveni hry a potrebnych promennych - hraci plocha, mod, symboly hracu
        self.board = [
            [" ", " ", " "],
            [" ", " ", " "],
            [" ", " ", " "]
        ]
        self.introduction() #privitani uzivatele, pripadne vypsani pravidel
        self.mode = mode #uzivatel v prubehu vybere mod hry (multi, random, minimax) 
        self.player1, self.player2 = player1, player2 #volba jakychkoliv symbolu pro reprezentaci
    
    def introduction(self): #uvodni predstaveni hry, animace, vypsani pravdiel
        os.system(clr) #smazani dosavadniho obsahu terminalu
        print("Welcome to the game TicTacToe!\n")
        self.printBoard() #funkce zobrazujici aktualni stav hraci plochy
        input(press_enter)
        time.sleep(0.5) #delay mezi jednotlivymi prechody
        os.system(clr)
        
        for i in range(3): #uvitaci animace pro tictactoe
            os.system(clr)
            self.board[i][i] = "o"
            print(ttt_heading)
            if i == 2:
                self.board[0][0], self.board[1][1], self.board[2][2] = self.board[0][0] + '\u0336', self.board[1][1] + '\u0336', self.board[2][2] + '\u0336'
            self.printBoard()
            time.sleep(1)
            
        self.board[0][0], self.board[1][1], self.board[2][2] = " ", " ", " " #uvedeni hraci plochy do uvodniho stavu
        
        print()
        input(press_enter)
        print(ttt_heading)
        
        rules = input("type [rules] if you dont know how to play \nelse press enter...\n:")
        if rules == "rules": #vypsani pravidel hry, pokud je uzivatelem pozadano
            print("Rules: Player 1 and player 2, often represented by X and O, take turns \nmarking the spaces in a 3x3 grid. The player who succeeds in placing \nthree of their marks in a horizontal, vertical, or diagonal row wins.")
            input(press_enter)
        os.system(clr)

        return
    
    def chooseSymbols(self): #vyber reprezentace tahu jednotlivych hracu, vraci do __init__ symboly pro reprezentaci
        print(ttt_heading)
        player1 = input("type some character for representation Player1.\n:")
        while player1 ==  " " or player1 == "": #pokud hrac1 vyplni pole neviditelnym znakem, pozada o nove vyplnení
            os.system(clr)
            print(ttt_heading)
            player1 = input("type other character for representation Player1.\n:")
        print(f"chosen symbol: {player1}\n")
        
        if self.mode == "multi": #vyber reprezentace druheho hrace dle vyberu modu
            player2 = input("type some character for representation Player2.\n:")
        elif self.mode == "random":
            player2 = input("type some character for representation Drunk VSE Student.\n:")
        else: 
            player2 = input("type some character for representation Matfyz Man.\n:")
        
        while player2 == " " or player2 == "" or player2 == player1: #pokud hrac2 ma svou reprezentaci neviditelny znak, nebo stejny jako hrac1
            os.system(clr)
            print(ttt_heading)
            print("YOU HAVE CHOSEN A SPACE CHAR OR SAME SYMBOL AS PLAYER1!")
            if self.mode == "multi":
                player2 = input("type other character for representation Player2.\n:")
            elif self.mode == "random":
                player2 = input("type other character for representation Drunk ZCU student.\n:")
            else: 
                player2 = input("type other character for representation Matfyz Man.\n:")
        
        print(f"chosen symbol: {player2}")
        
        return player1, player2
        
    def menu(self): #vyber hraciho modu, vrati do funkce __init__ hraci mod
        print(ttt_heading)
        mode = input("Type: \n[minimax] for single-player against minimax \n[multi] for multi-player \n[random] for single-player against radom \n[quit] for exit the game \n:")
        os.system(clr)
        while mode not in ["random", "minimax", "multi", "quit"]: #pokud odeslano neco jineho nez pozadovany mod, vcetne chybneho zadani
            os.system(clr)
            print(ttt_heading)
            mode = input("\nYOU PROLLY MISSTYPED, TRY AGAIN! \n[minimax] for single-player against minimax \n[multi] for multi-player \n[random] for single-player against radom \n[quit] for exit the game \n:")
        
        if mode == "quit":
            os.system(clr)
            print(ttt_heading) 
            print("Thank you for playing this game, looking forward to seeing you next time.")
            time.sleep(3)
            os.system(clr)
            quit()
        
        return mode
   
    def emptyBoard(self): #vrati odehranou hraci plochu do puvodniho stavu
        for i in range(3):
            self.board[i][0], self.board[i][1], self.board[i][2] = " ", " ", " "

        return
        
    def isEnd(self, board): #kontrola vyhry pro funkci minimax, ktera potrebuje vedet primo hrace, kvuli vypoctu hodnoty
        for i in range(3):
            if (board[i][0] != " ") and board[i][0] == board[i][1] == board[i][2]: #horizontalni
                return board[i][0], [i, 0] 
        
        for i in range(3):
            if (board[0][i] != " ") and board[0][i] == board[1][i] == board[2][i]: #veritkalni
                return board[0][i], [i, 1]
            
        if (board[0][0] != " ") and board[0][0] == board[1][1] == board[2][2]: #leva diagonala
            return board[0][0], [0, 2]
        if (board[0][2] != " ") and board[0][2] == board[1][1] == board[2][0]: #prava diagonala
            return board[0][2], [2, 2]
        
        for i in range(3): #kontrola remizi
            for j in range(3):
                if board[i][j] not in [self.player1, self.player2]:
                    return False, [None, None]
        
        return 1, [None, None]
    
    def showWinner(self, comb): #preskrtne vyherni kombinaci
        pos, kind = comb #comb je druha cast, kterou vrati funkce isEnd a ta obsahuje pole - pozici vyherni kombinace a druh vyherni kombinace
        if kind == 0: #horizontalni vyhra
            self.board[pos][0], self.board[pos][1], self.board[pos][2] = self.board[pos][0] + '\u0336', self.board[pos][1] + '\u0336', self.board[pos][2] + '\u0336'
        elif kind == 1: #vertikalni vyhra
            self.board[0][pos], self.board[1][pos], self.board[2][pos] = self.board[0][pos] + '\u0336', self.board[1][pos] + '\u0336', self.board[2][pos] + '\u0336'
        elif kind == 2:
            if pos == 0: #diagonalni vyhra
                self.board[0][0], self.board[1][1], self.board[2][2] = self.board[0][0] + '\u0336', self.board[1][1] + '\u0336', self.board[2][2] + '\u0336'
            else:
                self.board[0][2], self.board[1][1], self.board[2][0] = self.board[0][2] + '\u0336', self.board[1][1] + '\u0336', self.board[2][0] + '\u0336'
        else: pass
        
        return
        
    
    def whoStarts(self): #rozhodovaci funkce pro mod multi, jaky hrac bude zacinat tah
        print(ttt_heading)
        player = input("choose who will place first:") #ocekava zadani jedhono ze symbolu
        while player not in [self.player1, self.player2]: #dokud nebude zadan jeden z vybranych symbolu, prvni pokus zadan chybne
            os.system(clr)
            print("\n--> TIC TAC TOE <--\n")
            print(f"NOT VALID! chooose {self.player1} or {self.player2}")
            player = input(":")
        
        os.system(clr)
        return player
    
    def switchPlayer(self, player): #prohozeni tahu hracu
        if player == self.player1:
            return self.player2
        else:
            return self.player1
        
    def endGame(self, counter, player):
        os.system(clr)
        print()
        self.printBoard()
        if counter == 10: #rozhodnuti jakym stavem hra skoncila
            print("\nit is a tie!")
        else:
            print(f"\n{player} has won the game!")
            
        return
    
    def chooseMove(self, player): #vybirani tahu uzivatelu
        row = (input("choose the row [0-2]: ")) #pozice hraci plochy radek
        col = (input("choose the column [0-2]: ")) #pozice hraci plochy sloupec
        if row not in ["0", "1", "2"] or col not in ["0", "1", "2"]: #pokud hodnota je mimo rozsah plochy
            os.system(clr)
            print(ttt_heading)
            print("A PROBLEM OCCURED! \nrow/col out of board or you typed not integer character.")
            print(f"you play with {player}")
            self.printBoard()
            return True #hrac zadal spatne souradnice, funkce se bude opakovat
        
        row, col = int(row), int(col)
        if self.validMove(row, col): #pri zadani spravnych indexu hraci plochy kontrola zda-li je pole prazdne
            self.board[row][col] = player #umisteni tahu hrace na hraci plochu
            return False #souradnice jsou spravne a validni, hra pokracuje
        
        return True #hrac zadal spatne souradnice, funkce se bude opakovat
 
    def chooseMoveRandom(self, player): #v modu random, kdy pocitac vybere pseudonahodne pole
        row = rndm.randint(0, 2)
        col = rndm.randint(0, 2)
        if self.validMove(row, col): #pokud je pole obsazene, opakuje se nahodny vyber pole
            self.board[row][col] = player
            return False #pole spravne vybrane
        
        return True #pole spatne vybrane
    
    def min(self, player, board): 
        minworst = 2
        res, comb = self.isEnd(board)
        
        if res == self.player1:
            return [-1, 0, 0]
        elif res == self.player2:
            return [1, 0, 0]
        elif res == 1:
            return [0, 0, 0]
        
        for i in range(3):
            for j in range(3):
                if board[i][j] == " ":
                    board[i][j] = self.player1
                    (minmax, min_i, min_j) = self.max(player, board)
                    if minmax < minworst:
                        minworst, px, py = minmax, i, j
                    board[i][j] = " "
        return [minworst, px, py]
    
    def max(self, player, board):
        maxworst = -2
        res, comb = self.isEnd(board)
        
        if res == self.player1:
            return [-1, 0, 0]
        elif res == self.player2:
            return [1, 0, 0]
        elif res == 1:
            return [0, 0, 0]
        
        for i in range(3):
            for j in range(3):
                if board[i][j] == " ":
                    board[i][j] = self.player2
                    (minmax, min_i, min_j) = self.min(player, board)
                    if minmax > maxworst:
                        maxworst, px, py = minmax, i, j
                    board[i][j] = " "
        return [maxworst, px, py]
    
    def chooseMoveMinimax(self, player): #prerekvizita pro funkce mac a min algoritmu minimax, dle pocatecniho tahu spusti jednu funkci prvni
        board = self.board.copy() #kopie prozatim odehrane hry, pro kterou algorimus bude pocitat nejvhodnejsi tah
        if start != "F": #rozliseni prvni funkce dle poradni prvniho tahu
            minmax, px, py = self.min(player, board) #minimalizacni cast minimix algoritmu
        else:
            minmax, px, py = self.max(player, board) #maximalizacni cast minimax algoritmu
        self.board[px][py] = player #tah, ktery byl vypocitan jako nejvhodnejsi je zaznamenan
        
        return
            
    def validMove(self, row, col): #kontrola, zda neni tah umisten na obshazene pole, vrati bool hodnotu
        if self.board[row][col] in [self.player1, self.player2]:
            print("NOT VALID! the idex is already occupied.")
            return False
        
        return True
    
    def printBoard(self): #zobrazeni dosavadni odehrane hry do terminalu
        print("   0     1     2")
        print("  ----+-----+----")
        for i in range(3):
            print(i, "", self.board[i][0] , " | ", self.board[i][1], " | ", self.board[i][2])
        print("  ----+-----+----")
        
        return
    
    def multiGameStart(self): #hlavni funkce pro hru multi-player
        counter = 1 #pocet odehranych tahu - kvuli remize
        player = self.whoStarts() #rozhodnuti jaky hrac bude na rade jako prvni
            
        while counter < 10: #hlavni cyklus hry, dokud neni remiza, nebo vyhra
            print(ttt_heading)
            print(f"you play with {player}") #zobrazeni symbolu hrace, ktery je na tahu
            self.printBoard() #zobrazeni hraci plochy
            while self.chooseMove(player): pass #dokud nevybran validni tah
            res, comb = self.isEnd(self.board) #kontrola, zda-li jeden z hracu nevyhral, pak konec
            if res != False:
                self.showWinner(comb)
                break
            
            player = self.switchPlayer(player) #zmena poradi tahu hracu
            counter += 1 
            os.system(clr)
        
        #konec hry
        self.endGame(counter, player)
        
        return
        
    def minimax_randomGameStart(self): #hlavni funkce pro hru s minimax algoritmem/random pc
        global start #globalni start, kvuli funkci - hooseMoveMinimax - ktera rozhoduje jakou cast minimax algoritmu spusti prvni
        counter = 1
        os.system(clr)
        self.printBoard()
        start = input(f"your symbol is {self.player1}, if you want to start type [F].\n:") #rozhodnuti uzivatele zdali chce zacinat - F, vzdy je uzivatel player1
        os.system(clr)
        
        if start != "F": #rozhodnuti prvniho tahu, dle zadaneho vstupu uzivatelem
            player = self.player2
        else: 
            player = self.player1
        
        while counter < 10: #hlavni cyklus hry
            print(ttt_heading)
            print(f"you play with {player}")
            self.printBoard()
            if player == self.player1: #rozhodnuti kdo je na tahu prvni uzivatel/minimax
                while self.chooseMove(player): pass
            else:
                if self.mode == "random": 
                    while self.chooseMoveRandom(player): pass #pc random vyber
                else: self.chooseMoveMinimax(player) #minimax algoritmus
                
            res, comb = self.isEnd(self.board) #kontrola, zda-li jeden z hracu nevyhral, pak konec
            if res != False:
                self.showWinner(comb)
                break

            player = self.switchPlayer(player) #prohozeni tahu hracu
            counter += 1
            os.system(clr)
            
        #konec hry
        self.endGame(counter, player)
        
        return
            
#hlavni cyklus hry   
def playTicTacToe(): #nekonecny cyklus, ktery slouzi k predstaveni opetovanemu spusteni hry po konecnem stavu - vyhra/remiza
    while True: #cyklus ukoncem v nabidce menu, tim ze uzivatel napise quit
        g.mode = g.menu() #menu, kde si uzivatel vybira mezi mody - multi, random, minimax
        g.player1, g.player2 = g.chooseSymbols() #vyber reprezentace tahu jednotlivych hracu
        if g.mode == "multi": g.multiGameStart() #rozhodnuti dle toho, jaky mod hry si uzivatel vybral
        else: g.minimax_randomGameStart()
        g.emptyBoard() #uvedeni hraci plochy do puvodniho stavu - anulace
        
#global variables
ttt_heading = "\n--> TIC TAC TOE <--\n"
clr = "clear"
press_enter = "Press enter to continue...\n"

#exekuce programu
g = Game() #vytvoreni tridy hry a s tim inicializace - __init__
playTicTacToe() #inicializace funkce, ktera se stara o prubeh hry