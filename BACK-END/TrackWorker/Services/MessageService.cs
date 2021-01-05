using AutoMapper;
using System.Threading.Tasks;
using TrackDataAccess.Models;
using TrackDataAccess.Repositories;
using TrackWorker.Models;

namespace TrackWorker.Services {
    public class MessageService : IMessageService {

        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessageService(IMessageRepository messageRepository, IMapper mapper) {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(MessageModel reportModel) {
            var report = _mapper.Map<Message>(reportModel);
            _messageRepository.Add(report);
            await _messageRepository.SaveAsync();
        }
    }
}
