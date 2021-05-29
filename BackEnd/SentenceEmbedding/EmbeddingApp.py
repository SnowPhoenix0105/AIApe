import const
import EmbeddingModel
import argparse
import os
from flask import Flask, request, abort, make_response

parser = argparse.ArgumentParser(description='An app for sentences embedding.')
parser.add_argument('-mn', '--model_name', type=str, 
                    default='distillation-sts_2021-05-28_17-33-59', help='model name')
parser.add_argument('-msn', '--max_sentences_num', type=int, default=16, help='max sentences number')
parser.add_argument('-d', '--device', type=str, default='cuda:{}'.format(0), help='device to run model')

args = parser.parse_args()

embedding_model = EmbeddingModel.EmbeddingModel(model_name=args.model_name,
                                 max_sentences_num=args.max_sentences_num, device=args.device)

app = Flask('EmbeddingApp', root_path=const.CURRENT_PATH)

check_authentication = {'root': 'root@aiape.icu', 'lmx': 'lmx@aiape.icu', 'dxy': 'dxy@aiape.icu'}

@app.route('/api/embeddings', methods=['POST'])
def api_embeddings():
    res = {'status': '', 'message': '', 'embeddings': []}
    json_body = request.json
    if 'name' not in json_body or \
       'password' not in json_body or \
       json_body['password'] != check_authentication[json_body['name']]:
        res['status'] = 'fail'
        res['message'] = 'authentication failed'
        return make_response(res, 401)
    if 'sentences' not in json_body:
        res['status'] = 'fail'
        res['message'] = 'lack of sentences'
        return make_response(res, 200)
    embeddings = embedding_model.embedding(json_body['sentences']).astype(float)
    res['status'] = 'success'
    res['message'] = 'embedding succeeded'
    res['embeddings'] = [list(embedding) for embedding in embeddings]
    # print(res)
    return make_response(res, 200)

if __name__ == '__main__':
    app.run(debug=True)