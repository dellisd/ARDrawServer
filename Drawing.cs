namespace ARDrawServer
{
    public class Drawing
    {
        public int Id { get; set; }
        
        public decimal Longitude { get; set; }
        
        public decimal Latitude { get; set; }
        
        public decimal Altitude { get; set; }
        
        public decimal Bearing { get; set; }
        
        public int Color { get; set; }
        
        public string PathData { get; set; }
    }
}