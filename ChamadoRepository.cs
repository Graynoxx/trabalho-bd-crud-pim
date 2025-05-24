using System;
using System.Collections.Generic;
using Npgsql;

public class ChamadoRepository
{
    private string connectionString = "Host=localhost;Username=postgres;Password=senha;Database=suporte_ia";

    // CREATE
    public void CriarChamado(string titulo, string descricao, string categoria)
    {
        using var conn = new NpgsqlConnection(connectionString);
        conn.Open();

        string sql = "INSERT INTO chamados (titulo, descricao, categoria, status) VALUES (@titulo, @descricao, @categoria, 'Aberto')";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("titulo", titulo);
        cmd.Parameters.AddWithValue("descricao", descricao);
        cmd.Parameters.AddWithValue("categoria", categoria);
        cmd.ExecuteNonQuery();
    }

    // READ
    public List<string> ListarChamadosPorStatus(string status)
    {
        var chamados = new List<string>();
        using var conn = new NpgsqlConnection(connectionString);
        conn.Open();

        string sql = "SELECT id, titulo FROM chamados WHERE status = @status";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("status", status);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            chamados.Add($"#{reader.GetInt32(0)} - {reader.GetString(1)}");
        }

        return chamados;
    }

    // UPDATE
    public void AtualizarStatusChamado(int idChamado, string novoStatus)
    {
        using var conn = new NpgsqlConnection(connectionString);
        conn.Open();

        string sql = "UPDATE chamados SET status = @novoStatus WHERE id = @id";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", idChamado);
        cmd.Parameters.AddWithValue("novoStatus", novoStatus);
        cmd.ExecuteNonQuery();
    }
}
