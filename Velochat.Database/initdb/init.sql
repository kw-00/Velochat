CREATE SCHEMA app;
SEt search_path TO app;

CREATE TABLE identities (
    id SERIAL PRIMARY KEY,
    login TEXT NOT NULL UNIQUE,
    password_hash TEXT NOT NULL
);

CREATE TABLE friend_requests (
    sender_id INT NOT NULL,
    recipient_id INT NOT NULL,
    CONSTRAINT pk_invitations PRIMARY KEY (sender_id, recipient_id)
);

CREATE TABLE friendships (
    subject_1_id INT NOT NULL,
    subject_2_id INT NOT NULL,
    CONSTRAINT pk_friendships PRIMARY KEY (subject_1_id, subject_2_id)
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
    identity_id INT NOT NULL,
    status TEXT NOT NULL
);

/* Friendship */
ALTER TABLE friend_requests ADD CONSTRAINT fk_friend_requests_sender_id
    FOREIGN KEY (sender_id) REFERENCES identities (id)
;

ALTER TABLE friend_requests ADD CONSTRAINT fk_friend_requests_recipient_id
    FOREIGN KEY (recipient_id) REFERENCES identities (id)
;

CREATE UNIQUE INDEX no_two_way_friend_requests ON friend_requests (
    LEAST(sender_id, recipient_id), GREATEST(sender_id, recipient_id)
);

ALTER TABLE friendships ADD CONSTRAINT fk_friendships_subject_1_id
    FOREIGN KEY (subject_1_id) REFERENCES identities (id)
;

ALTER TABLE friendships ADD CONSTRAINT fk_friendships_subject_2_id
    FOREIGN KEY (subject_2_id) REFERENCES identities (id)
;

ALTER TABLE friendships ADD CONSTRAINT subject_order_check
    CHECK (subject_1_id < subject_2_id)
;

/* Rooms */
ALTER TABLE room_presences ADD CONSTRAINT fk_room_presences_room_id
    FOREIGN KEY (room_id) REFERENCES rooms (id)
;

ALTER TABLE room_presences ADD CONSTRAINT fk_room_presences_member_id
    FOREIGN KEY (member_id) REFERENCES identities (id)
;

ALTER TABLE rooms ADD CONSTRAINT fk_rooms_owner_id
    FOREIGN KEY (owner_id) REFERENCES identities (id)
;

ALTER TABLE rooms ADD CONSTRAINT unique_owner_and_name 
    UNIQUE (owner_id, name)
;

/* Chat */
ALTER TABLE chat_messages ADD CONSTRAINT fk_chat_messages_room_id
    FOREIGN KEY (room_id) REFERENCES rooms (id)
;

ALTER TABLE chat_messages ADD CONSTRAINT fk_chat_messages_author_id
    FOREIGN KEY (author_id) REFERENCES identities (id)
;

/* Refresh tokens */
ALTER TABLE refresh_token_states ADD CONSTRAINT fk_refresh_token_states_identity_id
    FOREIGN KEY (identity_id) REFERENCES identities (id)
;

ALTER TABLE refresh_token_states ADD CONSTRAINT refresh_token_states_status_check
    CHECK (status IN ('active', 'used', 'revoked'))
;
    

RESET search_path;