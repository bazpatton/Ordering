# Order Processing Service

Order processing with asynchronous event handling, retry mechanisms, and containerized deployment.

## API Endpoints

### Create Order
```http
POST /orders
Content-Type: application/json

{
  "customerId": "customer-123",
  "items": [
    {
      "sku": "PROD-001",
      "quantity": 2,
      "price": 29.99
    }
  ]
}
```

### Get Order
```http
GET /orders/{orderId}
```

### Health Check
```http
GET /health
```

### Build Image
```bash
docker build -t ordering-api .
```

### Run Container
```bash
docker compose up -d
```

### View Logs
```bash
docker compose logs -f
```

### Stop Services
```bash
docker compose down
```

## Testing the API

### Using curl

```bash
# Create an order
curl -X POST http://localhost:5000/orders \
  -H "Content-Type: application/json" \
  -d '{
    "customerId": "customer-123",
    "items": [
      {
        "sku": "PROD-001",
        "quantity": 2,
        "price": 29.99
      }
    ]
  }'

# Get order status
curl http://localhost:5000/orders/{orderId}

# Health check
curl http://localhost:5000/health

# Swagger
http://localhost:5000/swagger/index.html
```

## License

MIT License






