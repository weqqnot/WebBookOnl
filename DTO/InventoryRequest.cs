namespace WebBookShell.DTO
{
    public class InventoryRequest
    {
        public class AddBookRequest
        {
            public int BookId { get; set; }
            public int Quantity { get; set; }
        }
        public class UpdateQuantityRequest {
            public int BookId { get; set; }
            public int Quantity { get; set; }
        }

        public class TransferBookRequest
        {
            public int BookId { get; set; }
            public int Quantity { get; set; }
        }

    }
}
