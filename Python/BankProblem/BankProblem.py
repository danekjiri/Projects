class BVstrom:
    def __init__(self, id=None, castka=0):
        self.levy = None
        self.pravy = None
        self.id = id
        self.castka = castka 

    def pridat_ucet(self, id, castka=0):
        if not self.id:
            self.id = id
            self.castka = castka
            return

        if self.id == id:
            return "uz existuje"

        if id < self.id:
            if self.levy:
                zkus = self.levy.pridat_ucet(id, castka)
                if zkus == "uz existuje":
                    return "uz existuje"
                return
            
            self.levy = BVstrom(id, castka)
            return

        if self.pravy:
            zkus = self.pravy.pridat_ucet(id, castka)
            if zkus == "uz existuje":
                    return "uz existuje"
            return
        self.pravy = BVstrom(id, castka)

    def vhodne_id(self, momentalni):
        while momentalni.levy:
            momentalni = momentalni.levy
        return momentalni

    def zrus_ucet(self, id):
        global bvs
        rodic = None
        momentalni = self
        
        while momentalni.id != id:
            rodic = momentalni
            if momentalni.id == None:
                return "ucet neexistuje"
            
            if id < momentalni.id:
                momentalni = momentalni.levy
            else:
                momentalni  = momentalni.pravy
            
            if momentalni == None:
                return "ucet neexistuje"    
            
        if momentalni == None:
            return "ucet neexistuje"
        
        if momentalni.levy == None and momentalni.pravy == None:
            if momentalni != self:
                if rodic.levy == momentalni:
                    rodic.levy = None
                else:
                    rodic.pravy = None
            else:
                bvs = BVstrom()
            return

        elif momentalni.pravy and momentalni.levy:
            succ = self.vhodne_id(momentalni.pravy)
            self.zrus_ucet(succ.id)
            
            momentalni.id = succ.id
            momentalni.castka = succ.castka
            return
        
        else:
            if momentalni.levy:
                syn = momentalni.levy
            else:
                syn = momentalni.pravy
                
            if momentalni != self:
                if momentalni == rodic.levy:
                    rodic.levy = syn
                else:
                    rodic.pravy =  syn
            else:
                bvs = syn
        return 

    def najdi_a_zmen(self, id, castka, zmena):
        if self.id == None:
            return "ucet neexistuje"
        
        if id == self.id:
            if zmena == "plus":
                self.castka = int(self.castka) + int(castka)
                return
            elif zmena == "minus":
                if int(self.castka) < int(castka):
                    return "nizky stav uctu"
                else:
                    self.castka = int(self.castka) + (-int(castka))
                    return

        if id < self.id:
            if self.levy == None:
                return "ucet neexistuje"
            return self.levy.najdi_a_zmen(id, castka, zmena)

        if self.pravy == None:
            return "ucet neexistuje"
        return self.pravy.najdi_a_zmen(id, castka, zmena)

    def vypis_vzestupne(self, id):
        if self.levy != None:
            self.levy.vypis_vzestupne(id)
        if self.id != None:
            print(self.id, ":", self.castka, sep="", file=bankout)
        if self.pravy != None:
            self.pravy.vypis_vzestupne(id)

def rozhodni_a_vypis(id, operace, castka):
    if operace == "N":
        if bvs.pridat_ucet(id, castka) == "uz existuje":
            kontrola = "chyba: ucet uz existuje!"
            print(id, ":", operace, ":", castka, " ", kontrola, sep="", file=bankout)
        else:
            kontrola = "OK"
            print(id, ":", operace, ":", castka, " ", kontrola, sep="", file=bankout)
    elif operace == "Q":
        if bvs.zrus_ucet(id) == "ucet neexistuje":
            kontrola = "chyba: ucet neexistuje!"
            print(id, ":", operace, ":", castka, " ", kontrola, sep="", file=bankout)
        else: 
            kontrola = "OK"
            print(id, ":", operace, ":", castka, " ", kontrola, sep="", file=bankout)
    elif operace == "I":
        if bvs.najdi_a_zmen(id, castka, zmena="plus") == "ucet neexistuje":
            kontrola = "chyba: ucet neexistuje!"
            print(id, ":", operace, ":", castka, " ", kontrola, sep="", file=bankout)
        else:
            kontrola = "OK"
            print(id, ":", operace, ":", castka, " ", kontrola, sep="", file=bankout)
    elif operace == "D":
        zmena = bvs.najdi_a_zmen(id, castka, zmena="minus")
        if zmena == "ucet neexistuje":
            kontrola = "chyba: ucet neexistuje!"
            print(id, ":", operace, ":", castka, " ", kontrola, sep="", file=bankout)
        elif zmena == "nizky stav uctu":
            kontrola = "chyba: nizky stav uctu!"
            print(id, ":", operace, ":", castka, " ", kontrola, sep="", file=bankout)
        else:
            kontrola = "OK"
            print(id, ":", operace, ":", castka, " ", kontrola, sep="", file=bankout)
    
    
bankin = open("bank.in", "r")
bankout = open("bank.out", "w")
bvs = BVstrom()
cislo_dne = 1

for radek in bankin:
    print("===", cislo_dne, "===", file=bankout)
    
    vsechny_transakce = radek.split(";")
    vsechny_transakce.pop()
    for transakce in vsechny_transakce:
        id, operace, castka = transakce.split(":")
        if id[0] == "0":
            continue
        rozhodni_a_vypis(id, operace, castka)
        
    print("======", file=bankout)
    
    bvs.vypis_vzestupne(id)
    cislo_dne += 1
    
bankin.close()
bankout.close()
    