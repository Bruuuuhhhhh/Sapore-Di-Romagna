using System;
using System.Collections.Generic;

namespace SaporeDiRomagna.Data;

public enum PartnerType { AziendaAgricola, IndustriaTrasformazione, GDO, Vettore, ClienteDettaglio }
public enum DocumentType { DocumentoTrasporto, FatturaImmediata, FatturaDifferita, DocumentoEsterno }
public enum ServizioTipo { Pranzo, Cena }

public class Partner
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string RagioneSociale { get; set; } = string.Empty;
    public string PartitaIva { get; set; } = string.Empty;
    public string IndirizzoSedeLegale { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public PartnerType Tipo { get; set; }
}

public class Prodotto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public string Descrizione { get; set; } = string.Empty;
    public string UnitaMisura { get; set; } = "cad"; // kg, cad, pers
    public decimal PrezzoUnitario { get; set; }
    public decimal AliquotaIva { get; set; } = 22; // 10 or 22
    
    // Organizzazione Menu Ristorante
    public bool InMenu { get; set; } = false;
    public string CategoriaMenu { get; set; } = "Da Assegnare";
    
    // Gestione ingredienti
    public List<Guid> IngredientiIds { get; set; } = new();
}

public class Ingrediente
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public string Categoria { get; set; } = "Varie";
    public string Descrizione { get; set; } = string.Empty;
    public string UnitaMisura { get; set; } = "kg"; // kg, lt, pz
    public int Ordine { get; set; }
}

public class DocumentoRiga
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProdottoId { get; set; }
    public Prodotto? ProdottoInfo { get; set; }
    public int Quantita { get; set; } = 1;
    public decimal PrezzoUnitario { get; set; }
    public decimal TotaleRiga => PrezzoUnitario * Quantita;
}

public class Documento
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string NumeroDocumento { get; set; } = string.Empty;
    public DateTime Data { get; set; } = DateTime.Now;
    public DocumentType TipoDocumento { get; set; }
    public Guid PartnerId { get; set; }
    public bool IsAcquisto { get; set; }
    public string? UploadedFileName { get; set; }
    
    // For DDT
    public string VettoreNome { get; set; } = string.Empty;
    public string VettoreTarga { get; set; } = string.Empty;
    public TimeOnly? OraInizioTrasporto { get; set; }
    public string CausaleTrasporto { get; set; } = "Vendita merce";
    public string Imballaggio { get; set; } = "A perdere";
    public string Consegna { get; set; } = "Porto franco";
    public string Trasporto { get; set; } = "A mezzo ns. mittente";
    public string Pagamento { get; set; } = "Contanti/Bonifico";
    public string Note { get; set; } = "Contributo Ambientale CONAI assolto.";
    
    // Extra specific fields
    public int Colli { get; set; } = 1;
    public string Peso { get; set; } = string.Empty;
    public string RifDDT { get; set; } = string.Empty;
    public decimal SpeseAggiuntive { get; set; } = 0;

    // For Invoice totals
    public decimal TotaleImponibile { get; set; }
    public decimal TotaleIva { get; set; }
    public decimal TotaleFattura { get; set; }
    public decimal ScontoApplicato { get; set; } // Per gestire lo sconto 100% omaggio
    public bool StatusPagato { get; set; }

    public List<DocumentoRiga> Righe { get; set; } = new();
}

public class Prenotazione
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Data { get; set; } = DateTime.Today;
    public TimeOnly Orario { get; set; }
    public ServizioTipo Servizio { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public int Posti { get; set; } = 1;
    public string Note { get; set; } = string.Empty;
}

public class DatiAzienda
{
    public string RagioneSociale { get; set; } = "Sapore di Romagna S.r.l.";
    public string BrandName { get; set; } = "Sapore di Romagna - Cucina & Pizzeria km 0";
    public string PartitaIva { get; set; } = "IT08429310401";
    public string Indirizzo { get; set; } = "Corso Sozzi, 24 – 47521 Cesena (FC)";
    public string Telefono { get; set; } = "0547 123456";
    public string IBAN { get; set; } = "IT02A0123451234512345678901";
    public string PEC { get; set; } = "saporediromagna.srl@gmail.com";
    public string CodiceSDI { get; set; } = "4SUS67F";
    public string CapitaleSociale { get; set; } = "€ 10.000,00 i.v.";
    public string LinkDrive {get; set; } = "https://drive.google.com/drive/folders/1UbPlHvRydqpidxhfNLv28jcxShEVgOxO?usp=sharing";
}

public class AppState
{
    public bool IsLoggedIn { get; set; } = false;
    public DatiAzienda DatiAziendali { get; set; } = new();
    public List<Partner> Partners { get; set; } = new();
    public List<Prodotto> Prodotti { get; set; } = new();
    public List<Documento> Documenti { get; set; } = new();
    public List<Prenotazione> Prenotazioni { get; set; } = new();

    public List<Ingrediente> Ingredienti { get; set; } = new();

    public AppState()
    {
        // Init Mock Data
        
        // --- INGREDIENTI / MATERIE PRIME ---
        int ord = 1;
        
        // Carni e Salumi
        const string CAT_CARNI = "Carni e Salumi";
        AddIng("Macinato di manzo", "kg", ord++, CAT_CARNI);
        AddIng("Macinato di maiale", "kg", ord++, CAT_CARNI);
        AddIng("Controfiletto di manzo", "kg", ord++, CAT_CARNI);
        AddIng("Lombo di maiale", "kg", ord++, CAT_CARNI);
        AddIng("Salsiccia di maiale", "kg", ord++, CAT_CARNI);
        AddIng("Costine di maiale", "kg", ord++, CAT_CARNI);
        AddIng("Fegatelli", "kg", ord++, CAT_CARNI);
        AddIng("Castrato", "kg", ord++, CAT_CARNI);
        AddIng("Pancetta tesa", "kg", ord++, CAT_CARNI);
        AddIng("Pancetta per taglieri", "kg", ord++, CAT_CARNI);
        AddIng("Prosciutto crudo", "kg", ord++, CAT_CARNI);
        AddIng("Prosciutto cotto", "kg", ord++, CAT_CARNI);
        AddIng("Salame dolce", "kg", ord++, CAT_CARNI);
        AddIng("Salame piccante", "kg", ord++, CAT_CARNI);
        AddIng("Coppa", "kg", ord++, CAT_CARNI);
        AddIng("Mortadella", "kg", ord++, CAT_CARNI);
        
        // Latticini e Uova
        const string CAT_LATTE = "Latticini e Uova";
        AddIng("Uova fresche", "pz", ord++, CAT_LATTE);
        AddIng("Mozzarella fior di latte", "kg", ord++, CAT_LATTE);
        AddIng("Squacquerone", "kg", ord++, CAT_LATTE);
        AddIng("Ricotta", "kg", ord++, CAT_LATTE);
        AddIng("Mascarpone", "kg", ord++, CAT_LATTE);
        AddIng("Pecorino", "kg", ord++, CAT_LATTE);
        AddIng("Grana Padano", "kg", ord++, CAT_LATTE);
        AddIng("Parmigiano Reggiano", "kg", ord++, CAT_LATTE);
        AddIng("Panna da cucina", "lt", ord++, CAT_LATTE);
        AddIng("Latte intero", "lt", ord++, CAT_LATTE);
        AddIng("Burro", "kg", ord++, CAT_LATTE);
        
        // Farine e Dispensa
        const string CAT_DISPENSA = "Farine e Dispensa";
        AddIng("Farina 00", "kg", ord++, CAT_DISPENSA);
        AddIng("Lievito di birra", "kg", ord++, CAT_DISPENSA);
        AddIng("Pasta di semola", "kg", ord++, CAT_DISPENSA);
        AddIng("Pane casereccio", "kg", ord++, CAT_DISPENSA);
        AddIng("Passata di pomodoro", "kg", ord++, CAT_DISPENSA);
        AddIng("Zucchero", "kg", ord++, CAT_DISPENSA);
        AddIng("Caffè", "kg", ord++, CAT_DISPENSA);
        AddIng("Cioccolato fondente", "kg", ord++, CAT_DISPENSA);
        AddIng("Savoiardi o biscotti per base dolce", "kg", ord++, CAT_DISPENSA);
        AddIng("Riso", "kg", ord++, CAT_DISPENSA);
        
        // Ortofrutta e Aromi
        const string CAT_ORTO = "Ortofrutta e Aromi";
        AddIng("Patate", "kg", ord++, CAT_ORTO);
        AddIng("Pomodori freschi", "kg", ord++, CAT_ORTO);
        AddIng("Cipolla bionda", "kg", ord++, CAT_ORTO);
        AddIng("Cipolla rossa", "kg", ord++, CAT_ORTO);
        AddIng("Carote", "kg", ord++, CAT_ORTO);
        AddIng("Sedano", "kg", ord++, CAT_ORTO);
        AddIng("Aglio", "kg", ord++, CAT_ORTO);
        AddIng("Rucola", "kg", ord++, CAT_ORTO);
        AddIng("Lattuga", "kg", ord++, CAT_ORTO);
        AddIng("Radicchio", "kg", ord++, CAT_ORTO);
        AddIng("Zucchine", "kg", ord++, CAT_ORTO);
        AddIng("Melanzane", "kg", ord++, CAT_ORTO);
        AddIng("Peperoni", "kg", ord++, CAT_ORTO);
        AddIng("Funghi coltivati", "kg", ord++, CAT_ORTO);
        AddIng("Funghi porcini", "kg", ord++, CAT_ORTO);
        AddIng("Basilico fresco", "mz", ord++, CAT_ORTO);
        AddIng("Rosmarino", "mz", ord++, CAT_ORTO);
        AddIng("Prezzemolo", "mz", ord++, CAT_ORTO);
        AddIng("Origano secco", "gr", ord++, CAT_ORTO);
        
        // Condimenti e Varie
        const string CAT_VARIE = "Condimenti e Varie";
        AddIng("Olio Extravergine d'Oliva", "lt", ord++, CAT_VARIE);
        AddIng("Olio di semi", "lt", ord++, CAT_VARIE);
        AddIng("Vino bianco", "lt", ord++, CAT_VARIE);
        AddIng("Vino rosso", "lt", ord++, CAT_VARIE);
        AddIng("Sale grosso dolce di Cervia", "kg", ord++, CAT_VARIE);
        AddIng("Sale fino", "kg", ord++, CAT_VARIE);
        AddIng("Pepe nero", "gr", ord++, CAT_VARIE);
        AddIng("Noce moscata", "gr", ord++, CAT_VARIE);
        AddIng("Zafferano", "gr", ord++, CAT_VARIE);
        AddIng("Olio tartufato", "lt", ord++, CAT_VARIE);
        AddIng("Carciofini sott'olio", "kg", ord++, CAT_VARIE);
        AddIng("Olive nere", "kg", ord++, CAT_VARIE);
        AddIng("Aceto di vino", "lt", ord++, CAT_VARIE);
        AddIng("Aceto balsamico", "lt", ord++, CAT_VARIE);
        AddIng("Wurstel", "kg", ord++, CAT_CARNI);
        AddIng("Zucca", "kg", ord++, CAT_ORTO);
        AddIng("Base Sorbetto al Limone", "lt", ord++, CAT_VARIE);

        // --- PARTNER ---
        Partners.Add(new Partner { RagioneSociale = "Naturalia smsp", PartitaIva = "01122334455", IndirizzoSedeLegale = "Via delle Camelie 12, Cesena", Email = "ordini@naturalia.it", Tipo = PartnerType.AziendaAgricola });
        Partners.Add(new Partner { RagioneSociale = "la fabbrica del gusto", PartitaIva = "02233445566", IndirizzoSedeLegale = "Via del Gusto 5, Forlì", Email = "info@fabbricadelgusto.it", Tipo = PartnerType.IndustriaTrasformazione });
        Partners.Add(new Partner { RagioneSociale = "vucumprà center", PartitaIva = "03344556677", IndirizzoSedeLegale = "Viale Adriatico 100, Rimini", Email = "center@vucumpra.it", Tipo = PartnerType.GDO });
        Partners.Add(new Partner { RagioneSociale = "hermes trasporti srl", PartitaIva = "04455667788", IndirizzoSedeLegale = "Interporto, Cesena", Email = "logistica@hermes.it", Tipo = PartnerType.Vettore });

        // --- PRODOTTI (MENU RISTORANTE) ---
        
        // Antipasti
        Prodotti.Add(new Prodotto { Nome = "Crostini al ragù", PrezzoUnitario = 5.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Antipasti" });
        Prodotti.Add(new Prodotto { Nome = "Tagliere di salumi", PrezzoUnitario = 8.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Antipasti" });
        Prodotti.Add(new Prodotto { Nome = "Tagliere di formaggi", PrezzoUnitario = 8.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Antipasti" });
        Prodotti.Add(new Prodotto { Nome = "Tagliere misto", PrezzoUnitario = 9.50m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Antipasti" });
        
        // Primi
        Prodotti.Add(new Prodotto { Nome = "Tagliatelle al ragù", PrezzoUnitario = 9.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Primi" });
        Prodotti.Add(new Prodotto { Nome = "Tortellini al ragù", PrezzoUnitario = 9.50m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Primi" });
        Prodotti.Add(new Prodotto { Nome = "Rigatoni al ragù", PrezzoUnitario = 8.50m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Primi" });
        Prodotti.Add(new Prodotto { Nome = "Strozzapreti pasticciati", PrezzoUnitario = 8.50m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Primi" });
        Prodotti.Add(new Prodotto { Nome = "Strozzapreti al ragù", PrezzoUnitario = 9.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Primi" });
        Prodotti.Add(new Prodotto { Nome = "Tortellini alla Paulo Dybala", PrezzoUnitario = 10.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Primi" });
        Prodotti.Add(new Prodotto { Nome = "Cappellacci ai funghi porcini", PrezzoUnitario = 9.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Primi" });
        Prodotti.Add(new Prodotto { Nome = "Supplemento Grano Saraceno Bio", PrezzoUnitario = 2.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Primi" });

        // Secondi
        Prodotti.Add(new Prodotto { Nome = "Tagliata sale grosso di Cervia e rosmarino (500g)", PrezzoUnitario = 21.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Secondi" });
        Prodotti.Add(new Prodotto { Nome = "Tagliata rucola e grana (500g)", PrezzoUnitario = 22.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Secondi" });
        Prodotti.Add(new Prodotto { Nome = "Grigliata mista", PrezzoUnitario = 16.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Secondi" });

        // Contorni
        Prodotti.Add(new Prodotto { Nome = "Patate", PrezzoUnitario = 3.50m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Contorni" });
        Prodotti.Add(new Prodotto { Nome = "Verdure miste", PrezzoUnitario = 4.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Contorni" });
        Prodotti.Add(new Prodotto { Nome = "Insalata", PrezzoUnitario = 2.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Contorni" });

        // Pizze
        Prodotti.Add(new Prodotto { Nome = "Marinara", PrezzoUnitario = 4.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Pizze" });
        Prodotti.Add(new Prodotto { Nome = "Margherita", PrezzoUnitario = 5.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Pizze" });
        Prodotti.Add(new Prodotto { Nome = "Capricciosa", PrezzoUnitario = 6.50m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Pizze" });
        Prodotti.Add(new Prodotto { Nome = "Peperoni", PrezzoUnitario = 7.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Pizze" });
        Prodotti.Add(new Prodotto { Nome = "Salsiccia", PrezzoUnitario = 7.50m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Pizze" });
        Prodotti.Add(new Prodotto { Nome = "Salame piccante", PrezzoUnitario = 7.50m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Pizze" });
        Prodotti.Add(new Prodotto { Nome = "Maiala speciale della casa", PrezzoUnitario = 10.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Pizze" });
        Prodotti.Add(new Prodotto { Nome = "Wurstel", PrezzoUnitario = 7.50m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Pizze" });
        Prodotti.Add(new Prodotto { Nome = "Wurstel e patatine", PrezzoUnitario = 8.50m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Pizze" });
        Prodotti.Add(new Prodotto { Nome = "Prosciutto e funghi", PrezzoUnitario = 8.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Pizze" });
        Prodotti.Add(new Prodotto { Nome = "Tartufona", PrezzoUnitario = 9.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Pizze" });

        // Dolci
        Prodotti.Add(new Prodotto { Nome = "Tiramisù", PrezzoUnitario = 4.50m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Dolci" });
        Prodotti.Add(new Prodotto { Nome = "Mascarpone", PrezzoUnitario = 5.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Dolci" });
        Prodotti.Add(new Prodotto { Nome = "Sorbetto al ragù", PrezzoUnitario = 3.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Dolci" });

        // Bevande
        Prodotti.Add(new Prodotto { Nome = "Acqua Minerale (0.75 lt)", PrezzoUnitario = 2.20m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Bevande" });
        Prodotti.Add(new Prodotto { Nome = "Vino della Casa (0.5 lt)", PrezzoUnitario = 8.00m, AliquotaIva = 22, InMenu = true, CategoriaMenu = "Bevande" });
        Prodotti.Add(new Prodotto { Nome = "Birra alla Spina Media (0.5 lt)", PrezzoUnitario = 6.50m, AliquotaIva = 22, InMenu = true, CategoriaMenu = "Bevande" });
        Prodotti.Add(new Prodotto { Nome = "Bibite in Lattina (0.33 lt)", PrezzoUnitario = 3.50m, AliquotaIva = 22, InMenu = true, CategoriaMenu = "Bevande" });
        Prodotti.Add(new Prodotto { Nome = "Caffè", PrezzoUnitario = 1.50m, AliquotaIva = 22, InMenu = true, CategoriaMenu = "Bevande" });

        // Altro
        Prodotti.Add(new Prodotto { Nome = "Coperto", PrezzoUnitario = 1.50m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Altro" });

        // Menù Composti (Fissi)
        Prodotti.Add(new Prodotto { Nome = "Menù Business (Pranzo)", PrezzoUnitario = 14.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Menù Composti" });
        Prodotti.Add(new Prodotto { Nome = "Menù Degustazione Romagnolo", PrezzoUnitario = 22.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Menù Composti" });
        Prodotti.Add(new Prodotto { Nome = "Menù Ciccia (Carne)", PrezzoUnitario = 28.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Menù Composti" });
        Prodotti.Add(new Prodotto { Nome = "Menù Pizza", PrezzoUnitario = 15.00m, AliquotaIva = 10, InMenu = true, CategoriaMenu = "Menù Composti" });

        // --- DOCUMENTI ---
        // Lista vuota, verranno aggiunti manualmente dall'utente

        // --- PRENOTAZIONI MOCK ---
        Prenotazioni.Add(new Prenotazione { 
            Data = DateTime.Today, 
            Orario = new TimeOnly(12, 30), 
            Servizio = ServizioTipo.Pranzo, 
            NomeCliente = "Mario Rossi", 
            Posti = 4 
        });
        Prenotazioni.Add(new Prenotazione { 
            Data = DateTime.Today, 
            Orario = new TimeOnly(20, 30), 
            Servizio = ServizioTipo.Cena, 
            NomeCliente = "Anna Bianchi", 
            Posti = 10 
        });
        Prenotazioni.Add(new Prenotazione { 
            Data = DateTime.Today.AddDays(1), 
            Orario = new TimeOnly(13, 00), 
            Servizio = ServizioTipo.Pranzo, 
            NomeCliente = "Luca Verdi", 
            Posti = 2 
        });

        // --- ASSOCIAZIONE INGREDIENTI ---
        Link("Crostini al ragù", "Pane casereccio", "Macinato di manzo", "Macinato di maiale", "Passata di pomodoro", "Cipolla bionda", "Carote", "Sedano", "Olio Extravergine d'Oliva");
        Link("Tagliere di salumi", "Prosciutto crudo", "Salame dolce", "Coppa", "Pancetta per taglieri", "Mortadella");
        Link("Tagliere di formaggi", "Squacquerone", "Pecorino", "Grana Padano", "Parmigiano Reggiano");
        Link("Tagliere misto", "Prosciutto crudo", "Salame dolce", "Coppa", "Squacquerone", "Pecorino", "Grana Padano");
        
        var raguIngs = new[] { "Macinato di manzo", "Macinato di maiale", "Passata di pomodoro", "Sedano", "Carote", "Cipolla bionda" };
        Link("Tagliatelle al ragù", raguIngs.Append("Uova fresche").Append("Farina 00").ToArray());
        Link("Tortellini al ragù", raguIngs.Append("Uova fresche").Append("Farina 00").ToArray());
        Link("Strozzapreti al ragù", raguIngs.Append("Farina 00").ToArray());
        Link("Rigatoni al ragù", raguIngs.Append("Pasta di semola").ToArray());
        Link("Strozzapreti pasticciati", "Farina 00", "Passata di pomodoro", "Macinato di manzo", "Macinato di maiale", "Panna da cucina");
        Link("Tortellini alla Paulo Dybala", "Uova fresche", "Farina 00", "Panna da cucina", "Zafferano", "Prosciutto crudo");
        Link("Cappellacci ai funghi porcini", "Uova fresche", "Farina 00", "Zucca", "Ricotta", "Funghi porcini", "Burro", "Prezzemolo");
        
        Link("Tagliata sale grosso di Cervia e rosmarino (500g)", "Controfiletto di manzo", "Sale grosso dolce di Cervia", "Rosmarino", "Olio Extravergine d'Oliva");
        Link("Tagliata rucola e grana (500g)", "Controfiletto di manzo", "Rucola", "Grana Padano", "Olio Extravergine d'Oliva");
        Link("Grigliata mista", "Salsiccia di maiale", "Costine di maiale", "Fegatelli", "Castrato", "Lombo di maiale");
        
        Link("Patate", "Patate", "Rosmarino", "Sale fino", "Olio di semi");
        Link("Verdure miste", "Zucchine", "Melanzane", "Peperoni", "Olio Extravergine d'Oliva");
        Link("Insalata", "Lattuga", "Pomodori freschi", "Carote");
        
        var basePizza = new[] { "Farina 00", "Lievito di birra", "Sale fino", "Olio Extravergine d'Oliva" };
        Link("Marinara", basePizza.Append("Passata di pomodoro").Append("Aglio").Append("Origano secco").ToArray());
        Link("Margherita", basePizza.Append("Passata di pomodoro").Append("Mozzarella fior di latte").Append("Basilico fresco").ToArray());
        Link("Capricciosa", basePizza.Append("Passata di pomodoro").Append("Mozzarella fior di latte").Append("Prosciutto cotto").Append("Funghi coltivati").Append("Carciofini sott'olio").Append("Olive nere").ToArray());
        Link("Maiala speciale della casa", basePizza.Append("Passata di pomodoro").Append("Mozzarella fior di latte").Append("Salsiccia di maiale").Append("Salame piccante").Append("Wurstel").Append("Prosciutto cotto").ToArray());
        Link("Tartufona", basePizza.Append("Mozzarella fior di latte").Append("Funghi coltivati").Append("Olio tartufato").ToArray());
        
        Link("Tiramisù", "Mascarpone", "Uova fresche", "Zucchero", "Savoiardi o biscotti per base dolce", "Caffè", "Cioccolato fondente");
        Link("Mascarpone", "Mascarpone", "Uova fresche", "Zucchero", "Cioccolato fondente");
        Link("Sorbetto al ragù", "Base Sorbetto al Limone", "Sedano", "Carote", "Cipolla bionda");

        Documenti.Add(new Documento { NumeroDocumento = "1", Data = new DateTime(2026, 04, 04), TipoDocumento = DocumentType.FatturaImmediata, PartnerId = Partners[0].Id, IsAcquisto = false, TotaleFattura = new decimal(233.70), UploadedFileName="Fattura Cena Naturalia.pdf" });
        Documenti.Add(new Documento { NumeroDocumento = "2", Data = new DateTime(2026, 04, 17), TipoDocumento = DocumentType.FatturaImmediata, PartnerId = Partners[1].Id, IsAcquisto = false, TotaleFattura = new decimal(379.39), UploadedFileName="Fattura Cena La Fabbrica del Gusto.pdf" });
        Documenti.Add(new Documento { NumeroDocumento = "3", Data = new DateTime(2026, 04, 13), TipoDocumento = DocumentType.FatturaImmediata, PartnerId = Partners[3].Id, IsAcquisto = false, TotaleFattura = new decimal(1235.52), UploadedFileName="Fattura Cena Hermes.pdf" });
        Documenti.Add(new Documento { NumeroDocumento = "4", Data = new DateTime(2026, 04, 08), TipoDocumento = DocumentType.FatturaImmediata, PartnerId = Partners[0].Id, IsAcquisto = true, TotaleFattura = new decimal(38.22), UploadedFileName="FATTURA PER RISTORAZIONE naturalia.pdf" });
        Documenti.Add(new Documento { NumeroDocumento = "5", Data = new DateTime(2026, 04, 08), TipoDocumento = DocumentType.FatturaDifferita, PartnerId = Partners[2].Id, IsAcquisto = true, TotaleFattura = new decimal(768.86), UploadedFileName="sapori_romagna_fattura.pdf" });
        Documenti.Add(new Documento { NumeroDocumento = "6", Data = new DateTime(2026, 04, 08), TipoDocumento = DocumentType.DocumentoTrasporto, PartnerId = Partners[2].Id, IsAcquisto = true, UploadedFileName="sapori_romagna_DDT.pdf" });
        
        
    }

    private void Link(string prodNome, params string[] ingNomi)
    {
        var prod = Prodotti.FirstOrDefault(p => p.Nome.Equals(prodNome, StringComparison.OrdinalIgnoreCase));
        if (prod == null) return;
        foreach (var name in ingNomi)
        {
            var ing = Ingredienti.FirstOrDefault(i => i.Nome.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (ing != null) prod.IngredientiIds.Add(ing.Id);
        }
    }

    private void AddIng(string nome, string um, int ordine, string categoria)
    {
        Ingredienti.Add(new Ingrediente { Id = Guid.NewGuid(), Nome = nome, UnitaMisura = um, Ordine = ordine, Categoria = categoria });
    }
}
