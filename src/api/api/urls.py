from django.conf.urls import url, include
from redis.views import UnitMessagesView

# Wire up our API using automatic URL routing.
# Additionally, we include login URLs for the browsable API.
urlpatterns = [
	url(r'^api/unit-messages$', UnitMessagesView.as_view(), name='unit_messages_view')
]