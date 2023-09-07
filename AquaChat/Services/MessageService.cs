namespace AquaChat.Services;

public class MessageService
{
    public List<MessageDto> GetMessageDtosByChatId()
    {
        return new List<MessageDto>()
        {
            new MessageDto
            {
                Id = 1,
                Content = "hello",
                Created = DateTime.Now,
                Type = 0
            },
            new MessageDto
            {
                Id = 2,
                Content = "hello",
                Created = DateTime.Now,
                Type = 1
            },
            new MessageDto
            {
                Id = 3,
                Content = "hello",
                Created = DateTime.Now,
                Type = 2
            }
        };
    }
}