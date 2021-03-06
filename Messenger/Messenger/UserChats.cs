﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger
{
    class UserChats
    {
        public UserChats(string nick, Messenger messenger)
        {
            this.messenger = messenger;
            this.Nick = nick;
        }
        private Messenger messenger;
        private string Nick { get; }

        public List<string> ChatsWithPeople;
        public List<string> AllElsePeople;
        public List<string> SecretGroups;
        public List<string> UserGroups;
        public List<string> PublicGroups;
        public List<string> Invitations;

        public async Task FindAllChats()
        {
            //await FindChatsWithPeople();
            await FindAllElsePeople();
            await FindSecretGroups();
            await FindUserGroups();
            FindPublicGroups();
            await FindInvitations();
        }
        public async Task<List<PersonChat>> FindPersonChats()
        {
            return (await FileMaster.ReadAndDeserialize<PersonChat>
                (Path.Combine(messenger.Server.UsersPath, Nick, "peopleChatsBeen.json")))
                ?? new List<PersonChat>();
        }
        public async Task<List<string>> FindInvitations()
        {
            Invitations = await FileMaster.ReadAndDeserialize<string>
                (Path.Combine(messenger.Server.UsersPath, Nick, "invitation.json"));
            return Invitations;
        }
        public async Task<List<string>> FindUserGroups()
        {
            UserGroups = await FileMaster.ReadAndDeserialize<string>
                (Path.Combine(messenger.Server.UsersPath, Nick, "userGroups.json"));
            return UserGroups;
        }
        public async Task<List<string>> FindAllElsePeople()
        {
            await FindChatsWithPeople();
            AllElsePeople = ((await FileMaster.ReadAndDeserialize<UserNicknameAndPasswordAndIPs>
                (Path.Combine(messenger.Server.NicknamesAndPasswordsPath, "users.json")))
                ?? new List<UserNicknameAndPasswordAndIPs>())
                .Select(x => x.Nickname)
                .Where(x => x != Nick)
                .Except(ChatsWithPeople)
                .ToList();
            return AllElsePeople;
        }
        public async Task<List<string>> FindChatsWithPeople()
        {
            ChatsWithPeople = ((await FileMaster.ReadAndDeserialize<PersonChat>
                (Path.Combine(messenger.Server.UsersPath, Nick, "peopleChatsBeen.json")))
                ?? new List<PersonChat>())
                .Select(chat => chat.Nicknames[0] != Nick ? chat.Nicknames[0] : chat.Nicknames[1])
                .ToList();
            return ChatsWithPeople;
        }
        public async Task<List<string>> FindSecretGroups()
        {
            SecretGroups = await FileMaster.ReadAndDeserialize<string>
                (Path.Combine(messenger.Server.UsersPath, Nick, "secretGroups.json"));
            return SecretGroups;
        }
        public List<string> FindPublicGroups()
        {
            PublicGroups = FileMaster.GetDirectories(messenger.Server.PublicGroupPath)
                .Select(path => FileMaster.GetFileName(path))
                .ToList();
            return PublicGroups;
        }
    }
}
