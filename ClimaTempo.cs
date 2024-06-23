using Microsoft.AspNetCore.SignalR;

namespace PROJETO_CLIMA_API
{
    public class ClimaTempo
    {
        public string iTOKEN = "insira_o_token_aqui";

    }

    public class ClimaItem
    {
        public string Date { get; set; }
        public Temperature Temperature { get; set; }

        public string Description { get; set; } // Adicionando a descrição em "pt"
    }

    public class Temperature
    {
        public double Min { get; set; }
        public double Max { get; set; }
    }



    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public Data Data { get; set; }
    }

    public class Data
    {
        public int Temperature { get; set; }
        public string WindDirection { get; set; }
        public int WindVelocity { get; set; }
        public int Humidity { get; set; }
        public string Condition { get; set; }
        public int Pressure { get; set; }
        public string Icon { get; set; }
        public int Sensation { get; set; }
        public DateTime Date { get; set; }
    }




}

