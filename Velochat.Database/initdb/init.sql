CREATE SCHEMA app;
SEt search_path TO app;

CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    login TEXT NOT NULL UNIQUE,
    password_hash TEXT NOT NULL
);

CREATE TABLE friendships (
    initiator_id INT NOT NULL,
    receiver_id INT NOT NULL,
    accepted BOOLEAN NOT NULL,
    CONSTRAINT pk_invitations PRIMARY KEY (initiator_id, receiver_id)
);

CREATE TABLE room_presences (
    room_id INT NOT NULL,
    member_id INT NOT NULL,
    CONSTRAINT pk_room_presences PRIMARY KEY (room_id, member_id)
);

CREATE TABLE rooms (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL UNIQUE,
    owner_id INT NOT NULL
);

CREATE TABLE chat_messages (
    id SERIAL PRIMARY KEY,
    room_id INT NOT NULL,
    author_id INT NOT NULL,
    content TEXT NOT NULL
);

CREATE TABLE refresh_token_states (
    token TEXT PRIMARY KEY,
    user_id INT NOT NULL,
    status TEXT NOT NULL
);

/* Friendship */
ALTER TABLE friendships ADD CONSTRAINT fk_friendships_initiator_id
    FOREIGN KEY (initiator_id) REFERENCES users (id)
;

ALTER TABLE friendships ADD CONSTRAINT fk_friendships_receiver_id
    FOREIGN KEY (receiver_id) REFERENCES users (id)
;

ALTER TABLE friend_requests ADD CONSTRAINT fk_no_self_friendship
    CHECK (sender_id != receiver_id)
;

CREATE UNIQUE INDEX unique_friendship ON friendships (
    LEAST(initiator_id, receiver_id), GREATEST(initiator_id, receiver_id)
);

/* Rooms */
ALTER TABLE room_presences ADD CONSTRAINT fk_room_presences_room_id
    FOREIGN KEY (room_id) REFERENCES rooms (id)
;

ALTER TABLE room_presences ADD CONSTRAINT fk_room_presences_member_id
    FOREIGN KEY (member_id) REFERENCES users (id)
;

ALTER TABLE rooms ADD CONSTRAINT fk_rooms_owner_id
    FOREIGN KEY (owner_id) REFERENCES users (id)
;

ALTER TABLE rooms ADD CONSTRAINT unique_owner_and_name 
    UNIQUE (owner_id, name)
;

/* Chat */
ALTER TABLE chat_messages ADD CONSTRAINT fk_chat_messages_room_id
    FOREIGN KEY (room_id) REFERENCES rooms (id)
;

ALTER TABLE chat_messages ADD CONSTRAINT fk_chat_messages_author_id
    FOREIGN KEY (author_id) REFERENCES users (id)
;

/* Refresh tokens */
ALTER TABLE refresh_token_states ADD CONSTRAINT fk_refresh_token_states_user_id
    FOREIGN KEY (user_id) REFERENCES users (id)
;

ALTER TABLE refresh_token_states ADD CONSTRAINT refresh_token_states_status_check
    CHECK (status IN ('active', 'used', 'revoked'))
;
    

RESET search_path;