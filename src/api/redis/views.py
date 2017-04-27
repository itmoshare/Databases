from rest_framework.views import APIView
from rest_framework.response import Response
from rest_framework import status


class UnitMessagesView(APIView):

    def get(self, request, *args, **kw):
        # Process any get params that you may need
        # If you don't need to process get params,
        # you can skip this part
        get_arg1 = request.GET.get('arg1', None)
        get_arg2 = request.GET.get('arg2', None)

        response = Response('test', status=status.HTTP_200_OK)
        return response