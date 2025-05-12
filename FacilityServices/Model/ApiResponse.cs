namespace FacilityServices.Model
{
    public class ApiResponse<T>
    {
        public ApiResponse(bool success, int status, string message, T data) {
            this.success = success;
            this.status = status;
            this.message = message;
            this.data = data;
        }

        public bool success { get; set; } = false;
        public int status { get; set; }
        public string message { get; set; }
        public T? data { get; set; }
    }
}
