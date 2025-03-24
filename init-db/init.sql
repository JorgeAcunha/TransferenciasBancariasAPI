CREATE TABLE IF NOT EXISTS "CuentasBancarias" (
  "Id" SERIAL PRIMARY KEY,
  "NumeroCuenta" VARCHAR(50) NOT NULL,
  "Titular" VARCHAR(100) NOT NULL,
  "Saldo" DECIMAL(18,2) NOT NULL
);

CREATE TABLE IF NOT EXISTS "Transferencias" (
  "Id" SERIAL PRIMARY KEY,
  "CuentaOrigenId" INT NOT NULL,
  "CuentaDestinoId" INT NOT NULL,
  "Monto" DECIMAL(18,2) NOT NULL,
  "Fecha" TIMESTAMP NOT NULL,
  FOREIGN KEY ("CuentaOrigenId") REFERENCES "CuentasBancarias"("Id"),
  FOREIGN KEY ("CuentaDestinoId") REFERENCES "CuentasBancarias"("Id")
);

-- Inserta datos de prueba
INSERT INTO "CuentasBancarias" ("NumeroCuenta", "Titular", "Saldo")
VALUES
('1001', 'Jorge Usuario', 5000.00),
('1002', 'Maria Cliente', 3000.00);
