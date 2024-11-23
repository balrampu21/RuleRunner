using Renci.SshNet;

public class SftpHelper
{
    private readonly string _host;
    private readonly string _username;
    private readonly string _password;

    public SftpHelper(string host, string username, string password)
    {
        _host = host;
        _username = username;
        _password = password;
    }

    public void DownloadFile(string remoteFilePath, string localFilePath)
    {
        using var client = new SftpClient(_host, _username, _password);
        client.Connect();
        using var fileStream = File.OpenWrite(localFilePath);
        client.DownloadFile(remoteFilePath, fileStream);
        client.Disconnect();
    }

    public void UploadFile(string localFilePath, string remoteFilePath)
    {
        using var client = new SftpClient(_host, _username, _password);
        client.Connect();
        using var fileStream = File.OpenRead(localFilePath);
        client.UploadFile(fileStream, remoteFilePath, true);
        client.Disconnect();
    }
}
